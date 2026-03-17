using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Contracts.Routes;

namespace SchoolShuttleBus.Application.Routes;

public interface IRouteService
{
    Task<IReadOnlyCollection<RouteResponse>> GetRoutesAsync(CancellationToken cancellationToken);

    Task<Result<RouteResponse>> CreateRouteAsync(CreateRouteRequest request, CancellationToken cancellationToken);

    Task<Result<RouteResponse>> UpdateRouteAsync(Guid routeId, UpdateRouteRequest request, CancellationToken cancellationToken);

    Task<Result<RouteResponse>> UpsertStopsAsync(Guid routeId, UpsertRouteStopsRequest request, CancellationToken cancellationToken);

    Task<Result<RouteResponse>> AssignStaffAsync(Guid routeId, UpsertRouteAssignmentRequest request, CancellationToken cancellationToken);
}
