using Microsoft.EntityFrameworkCore;
using SchoolShuttleBus.Application.Attendance;
using SchoolShuttleBus.Application.Auth;
using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Contracts.Attendance;
using SchoolShuttleBus.Domain.Entities;
using SchoolShuttleBus.Domain.Shared;
using SchoolShuttleBus.Infrastructure.Persistence;

namespace SchoolShuttleBus.Infrastructure.Attendance;

internal sealed class AttendanceService(
    SchoolShuttleBusDbContext dbContext,
    ICurrentUserAccessor currentUserAccessor) : IAttendanceService
{
    public async Task<IReadOnlyCollection<AttendanceSessionResponse>> GetSessionsAsync(CancellationToken cancellationToken)
    {
        var sessions = await BuildSessionQuery().ToListAsync(cancellationToken);
        return sessions.Select(MapSession).ToArray();
    }

    public async Task<Result<AttendanceSessionResponse>> CreateSessionAsync(CreateAttendanceSessionRequest request, CancellationToken cancellationToken)
    {
        if (currentUserAccessor.UserId is not { } userId)
        {
            return Result<AttendanceSessionResponse>.Fail("Current user is unavailable.");
        }

        var route = await dbContext.Routes
            .Where(entity => entity.Id == request.RouteId)
            .Select(entity => new
            {
                entity.Direction,
                IsAllowed = currentUserAccessor.IsInRole(RoleNames.Administrator) ||
                            entity.Assignments.Any(assignment => assignment.StaffProfile.UserId == userId)
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (route is null || !route.IsAllowed)
        {
            return Result<AttendanceSessionResponse>.Fail("Route not found or access denied.");
        }

        if (route.Direction != request.Direction)
        {
            return Result<AttendanceSessionResponse>.Fail("Selected route does not match the requested trip direction.");
        }

        var existingSession = await BuildSessionQuery()
            .SingleOrDefaultAsync(session => session.RouteId == request.RouteId &&
                                             session.Date == request.Date &&
                                             session.Direction == request.Direction,
                cancellationToken);

        if (existingSession is not null)
        {
            return Result<AttendanceSessionResponse>.Ok(MapSession(existingSession));
        }

        var staffProfileId = await dbContext.StaffProfiles
            .Where(profile => profile.UserId == userId)
            .Select(profile => profile.Id)
            .SingleAsync(cancellationToken);

        var registrations = await dbContext.RideRegistrations
            .Include(registration => registration.Student)
                .ThenInclude(student => student.GuardianLinks)
                    .ThenInclude(link => link.Guardian)
            .Where(registration => registration.RouteId == request.RouteId &&
                                   registration.Date == request.Date &&
                                   registration.Direction == request.Direction &&
                                   registration.IsRegistered)
            .ToListAsync(cancellationToken);

        if (registrations.Count == 0)
        {
            return Result<AttendanceSessionResponse>.Fail("No registered students found for the selected route, date, and direction.");
        }

        var session = new AttendanceSession
        {
            RouteId = request.RouteId,
            Date = request.Date,
            Direction = request.Direction,
            CreatedByStaffProfileId = staffProfileId,
            Records = registrations.Select(registration => new AttendanceRecord
            {
                StudentId = registration.StudentId,
                Status = registration.IsPresent ? AttendanceStatus.Present : AttendanceStatus.Pending,
                EmergencyPhoneSnapshot = registration.Student.GuardianLinks
                    .OrderByDescending(link => link.IsPrimaryContact)
                    .Select(link => link.Guardian.PhoneNumber)
                    .FirstOrDefault() ?? string.Empty
            }).ToList()
        };

        dbContext.AttendanceSessions.Add(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        var created = await BuildSessionQuery().SingleAsync(entity => entity.Id == session.Id, cancellationToken);
        return Result<AttendanceSessionResponse>.Ok(MapSession(created));
    }

    public async Task<Result<AttendanceRecordResponse>> UpdateRecordAsync(Guid recordId, UpdateAttendanceRecordRequest request, CancellationToken cancellationToken)
    {
        var record = await dbContext.AttendanceRecords
            .Include(entity => entity.Student)
            .SingleOrDefaultAsync(entity => entity.Id == recordId, cancellationToken);

        if (record is null)
        {
            return Result<AttendanceRecordResponse>.Fail("Attendance record not found.");
        }

        record.Status = request.Status;
        record.UpdatedAtUtc = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<AttendanceRecordResponse>.Ok(new AttendanceRecordResponse(record.Id, record.StudentId, record.Student.FullName, record.Status, record.EmergencyPhoneSnapshot));
    }

    public async Task<Result<AttendanceSessionResponse>> CompleteSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        var session = await dbContext.AttendanceSessions.SingleOrDefaultAsync(entity => entity.Id == sessionId, cancellationToken);
        if (session is null)
        {
            return Result<AttendanceSessionResponse>.Fail("Attendance session not found.");
        }

        session.IsCompleted = true;
        session.UpdatedAtUtc = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        var completed = await BuildSessionQuery().SingleAsync(entity => entity.Id == sessionId, cancellationToken);
        return Result<AttendanceSessionResponse>.Ok(MapSession(completed));
    }

    private IQueryable<AttendanceSession> BuildSessionQuery()
    {
        var query = dbContext.AttendanceSessions
            .Include(session => session.Route)
            .Include(session => session.Records)
                .ThenInclude(record => record.Student)
            .AsQueryable();

        if (currentUserAccessor.IsInRole(RoleNames.Teacher) && currentUserAccessor.UserId is { } userId)
        {
            query = query.Where(session => session.Route.Assignments.Any(assignment => assignment.StaffProfile.UserId == userId));
        }

        return query;
    }

    private static AttendanceSessionResponse MapSession(AttendanceSession session)
    {
        return new AttendanceSessionResponse(
            session.Id,
            session.RouteId,
            session.Route.RouteName,
            session.Date,
            session.Direction,
            session.IsCompleted,
            session.Records.Select(record => new AttendanceRecordResponse(
                record.Id,
                record.StudentId,
                record.Student.FullName,
                record.Status,
                record.EmergencyPhoneSnapshot)).ToArray());
    }
}
