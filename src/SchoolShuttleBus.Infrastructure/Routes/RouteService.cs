using Microsoft.EntityFrameworkCore;
using SchoolShuttleBus.Application.Auth;
using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Application.Routes;
using SchoolShuttleBus.Contracts.Routes;
using SchoolShuttleBus.Domain.Entities;
using SchoolShuttleBus.Domain.Shared;
using SchoolShuttleBus.Infrastructure.Persistence;

namespace SchoolShuttleBus.Infrastructure.Routes;

internal sealed class RouteService(
    SchoolShuttleBusDbContext dbContext,
    ICurrentUserAccessor currentUserAccessor) : IRouteService
{
    public async Task<IReadOnlyCollection<RouteResponse>> GetRoutesAsync(CancellationToken cancellationToken)
    {
        var query = dbContext.Routes
            .Include(route => route.Stops.OrderBy(stop => stop.Sequence))
            .Include(route => route.Assignments)
                .ThenInclude(assignment => assignment.StaffProfile)
            .AsQueryable();

        if (currentUserAccessor.IsInRole(RoleNames.Teacher) && currentUserAccessor.UserId is { } userId)
        {
            query = query.Where(route => route.Assignments.Any(assignment => assignment.StaffProfile.UserId == userId));
        }

        var routes = await query.OrderBy(route => route.RouteName).ToListAsync(cancellationToken);
        return routes.Select(MapRoute).ToArray();
    }

    public async Task<Result<RouteResponse>> CreateRouteAsync(CreateRouteRequest request, CancellationToken cancellationToken)
    {
        var route = new Route
        {
            RouteName = request.RouteName,
            RouteType = request.RouteType,
            Direction = request.Direction,
            CampusName = request.CampusName
        };

        dbContext.Routes.Add(route);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<RouteResponse>.Ok(MapRoute(route));
    }

    public async Task<Result<RouteResponse>> UpdateRouteAsync(Guid routeId, UpdateRouteRequest request, CancellationToken cancellationToken)
    {
        var route = await dbContext.Routes
            .Include(entity => entity.Stops)
            .Include(entity => entity.Assignments)
                .ThenInclude(assignment => assignment.StaffProfile)
            .SingleOrDefaultAsync(entity => entity.Id == routeId, cancellationToken);

        if (route is null)
        {
            return Result<RouteResponse>.Fail("Route not found.");
        }

        route.RouteName = request.RouteName;
        route.RouteType = request.RouteType;
        route.IsActive = request.IsActive;
        route.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<RouteResponse>.Ok(MapRoute(route));
    }

    public async Task<Result<RouteResponse>> UpsertStopsAsync(Guid routeId, UpsertRouteStopsRequest request, CancellationToken cancellationToken)
    {
        var route = await dbContext.Routes
            .Include(entity => entity.Stops)
            .Include(entity => entity.Assignments)
                .ThenInclude(assignment => assignment.StaffProfile)
            .SingleOrDefaultAsync(entity => entity.Id == routeId, cancellationToken);

        if (route is null)
        {
            return Result<RouteResponse>.Fail("Route not found.");
        }

        dbContext.RouteStops.RemoveRange(route.Stops);
        route.Stops.Clear();

        foreach (var stop in request.Stops.OrderBy(stop => stop.Sequence))
        {
            route.Stops.Add(new RouteStop
            {
                RouteId = route.Id,
                Sequence = stop.Sequence,
                StopName = stop.StopName,
                Address = stop.Address,
                HandoffContactName = stop.HandoffContactName,
                HandoffContactPhone = stop.HandoffContactPhone
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<RouteResponse>.Ok(MapRoute(route));
    }

    public async Task<Result<RouteResponse>> AssignStaffAsync(Guid routeId, UpsertRouteAssignmentRequest request, CancellationToken cancellationToken)
    {
        var route = await dbContext.Routes
            .Include(entity => entity.Assignments)
            .ThenInclude(assignment => assignment.StaffProfile)
            .Include(entity => entity.Stops)
            .SingleOrDefaultAsync(entity => entity.Id == routeId, cancellationToken);

        if (route is null)
        {
            return Result<RouteResponse>.Fail("Route not found.");
        }

        if (route.Assignments.All(assignment => assignment.StaffProfileId != request.StaffProfileId))
        {
            route.Assignments.Add(new RouteAssignment
            {
                RouteId = routeId,
                StaffProfileId = request.StaffProfileId
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        route = await dbContext.Routes
            .Include(entity => entity.Assignments)
                .ThenInclude(assignment => assignment.StaffProfile)
            .Include(entity => entity.Stops)
            .SingleAsync(entity => entity.Id == routeId, cancellationToken);

        return Result<RouteResponse>.Ok(MapRoute(route));
    }

    private static RouteResponse MapRoute(Route route)
    {
        return new RouteResponse(
            route.Id,
            route.RouteName,
            route.RouteType,
            route.Direction,
            route.CampusName,
            route.IsActive,
            route.Stops.OrderBy(stop => stop.Sequence)
                .Select(stop => new RouteStopResponse(stop.Id, stop.Sequence, stop.StopName, stop.Address, stop.HandoffContactName, stop.HandoffContactPhone))
                .ToArray(),
            route.Assignments.Select(assignment => new RouteAssignmentResponse(assignment.Id, assignment.StaffProfileId, assignment.StaffProfile.FullName)).ToArray());
    }
}
