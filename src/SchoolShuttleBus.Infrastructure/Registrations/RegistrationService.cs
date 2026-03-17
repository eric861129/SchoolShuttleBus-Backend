using Microsoft.EntityFrameworkCore;
using SchoolShuttleBus.Application.Auth;
using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Application.Registrations;
using SchoolShuttleBus.Contracts.Registrations;
using SchoolShuttleBus.Domain.Entities;
using SchoolShuttleBus.Domain.Registrations;
using SchoolShuttleBus.Domain.Shared;
using SchoolShuttleBus.Infrastructure.Persistence;

namespace SchoolShuttleBus.Infrastructure.Registrations;

internal sealed class RegistrationService(
    SchoolShuttleBusDbContext dbContext,
    ICurrentUserAccessor currentUserAccessor) : IRegistrationService
{
    public async Task<Result<WeeklyRegistrationResponse>> GetWeekAsync(Guid studentId, DateOnly weekStart, CancellationToken cancellationToken)
    {
        var student = await AuthorizeStudentAccessAsync(studentId, cancellationToken);
        if (student is null)
        {
            return Result<WeeklyRegistrationResponse>.Fail("Student not found or access denied.");
        }

        return Result<WeeklyRegistrationResponse>.Ok(await BuildWeeklyResponseAsync(student, weekStart, cancellationToken));
    }

    public async Task<Result<WeeklyRegistrationResponse>> UpdateWeekAsync(UpdateWeeklyRegistrationRequest request, CancellationToken cancellationToken)
    {
        var student = await AuthorizeStudentAccessAsync(request.StudentId, cancellationToken);
        if (student is null)
        {
            return Result<WeeklyRegistrationResponse>.Fail("Student not found or access denied.");
        }

        RegistrationWeek.Create(request.WeekStart);
        var existing = await dbContext.RideRegistrations
            .Where(registration => registration.StudentId == request.StudentId &&
                                   registration.Date >= request.WeekStart &&
                                   registration.Date <= request.WeekStart.AddDays(4))
            .ToListAsync(cancellationToken);

        foreach (var day in request.Days)
        {
            UpsertTrip(existing, request.StudentId, day.Date, TripDirection.ToSchool, day.ToSchool, day.ToSchoolRouteId);
            UpsertTrip(existing, request.StudentId, day.Date, TripDirection.Homebound, day.Homebound, day.HomeboundRouteId);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<WeeklyRegistrationResponse>.Ok(await BuildWeeklyResponseAsync(student, request.WeekStart, cancellationToken));
    }

    public async Task<Result<WeeklyRegistrationResponse>> CopyLastWeekAsync(Guid studentId, DateOnly weekStart, CancellationToken cancellationToken)
    {
        var student = await AuthorizeStudentAccessAsync(studentId, cancellationToken);
        if (student is null)
        {
            return Result<WeeklyRegistrationResponse>.Fail("Student not found or access denied.");
        }

        RegistrationWeek.Create(weekStart);
        var previousWeekStart = weekStart.AddDays(-7);
        var sourceRegistrations = await dbContext.RideRegistrations
            .Where(registration => registration.StudentId == studentId &&
                                   registration.Date >= previousWeekStart &&
                                   registration.Date <= previousWeekStart.AddDays(4))
            .ToListAsync(cancellationToken);

        var targetRegistrations = await dbContext.RideRegistrations
            .Where(registration => registration.StudentId == studentId &&
                                   registration.Date >= weekStart &&
                                   registration.Date <= weekStart.AddDays(4))
            .ToListAsync(cancellationToken);

        foreach (var source in sourceRegistrations)
        {
            var targetDate = source.Date.AddDays(7);
            UpsertTrip(targetRegistrations, studentId, targetDate, source.Direction, source.IsRegistered, source.RouteId);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<WeeklyRegistrationResponse>.Ok(await BuildWeeklyResponseAsync(student, weekStart, cancellationToken));
    }

    public async Task<Result<StudentRegistrationSummaryResponse>> GetSummaryAsync(Guid studentId, CancellationToken cancellationToken)
    {
        var student = await AuthorizeStudentAccessAsync(studentId, cancellationToken);
        if (student is null)
        {
            return Result<StudentRegistrationSummaryResponse>.Fail("Student not found or access denied.");
        }

        var registrations = await dbContext.RideRegistrations
            .Where(registration => registration.StudentId == studentId)
            .ToListAsync(cancellationToken);

        return Result<StudentRegistrationSummaryResponse>.Ok(new StudentRegistrationSummaryResponse(
            student.Id,
            student.FullName,
            registrations.Count(registration => registration.IsRegistered),
            registrations.Count(registration => registration.IsPresent),
            student.Stage));
    }

    private void UpsertTrip(
        ICollection<RideRegistration> registrations,
        Guid studentId,
        DateOnly date,
        TripDirection direction,
        bool isRegistered,
        Guid? routeId)
    {
        var registration = registrations.SingleOrDefault(item => item.Date == date && item.Direction == direction);
        if (registration is null)
        {
            var created = new RideRegistration
            {
                StudentId = studentId,
                Date = date,
                Direction = direction,
                IsRegistered = isRegistered,
                RouteId = routeId
            };

            registrations.Add(created);
            dbContext.RideRegistrations.Add(created);

            return;
        }

        registration.IsRegistered = isRegistered;
        registration.RouteId = routeId;
        registration.UpdatedAtUtc = DateTimeOffset.UtcNow;
    }

    private async Task<Student?> AuthorizeStudentAccessAsync(Guid studentId, CancellationToken cancellationToken)
    {
        if (currentUserAccessor.UserId is not { } userId)
        {
            return null;
        }

        var query = dbContext.Students
            .Include(student => student.GuardianLinks)
                .ThenInclude(link => link.Guardian)
            .Where(student => student.Id == studentId);

        if (currentUserAccessor.IsInRole(RoleNames.Administrator))
        {
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        if (currentUserAccessor.IsInRole(RoleNames.Student))
        {
            return await query.SingleOrDefaultAsync(student => student.UserId == userId, cancellationToken);
        }

        if (currentUserAccessor.IsInRole(RoleNames.Parent))
        {
            return await query.SingleOrDefaultAsync(
                student => student.GuardianLinks.Any(link => link.Guardian.UserId == userId),
                cancellationToken);
        }

        return null;
    }

    private async Task<WeeklyRegistrationResponse> BuildWeeklyResponseAsync(Student student, DateOnly weekStart, CancellationToken cancellationToken)
    {
        RegistrationWeek.Create(weekStart);
        var registrations = await dbContext.RideRegistrations
            .Where(registration => registration.StudentId == student.Id &&
                                   registration.Date >= weekStart &&
                                   registration.Date <= weekStart.AddDays(4))
            .ToListAsync(cancellationToken);

        var days = Enumerable.Range(0, 5)
            .Select(offset =>
            {
                var date = weekStart.AddDays(offset);
                var toSchool = registrations.SingleOrDefault(item => item.Date == date && item.Direction == TripDirection.ToSchool);
                var homebound = registrations.SingleOrDefault(item => item.Date == date && item.Direction == TripDirection.Homebound);
                return new RegistrationDayResponse(
                    date,
                    toSchool?.IsRegistered ?? false,
                    homebound?.IsRegistered ?? false,
                    toSchool?.RouteId,
                    homebound?.RouteId);
            })
            .ToArray();

        return new WeeklyRegistrationResponse(student.Id, student.FullName, weekStart, days);
    }
}
