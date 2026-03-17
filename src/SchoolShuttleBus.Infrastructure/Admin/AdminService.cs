using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SchoolShuttleBus.Application.Admin;
using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Application.Dispatching;
using SchoolShuttleBus.Contracts.Admin;
using SchoolShuttleBus.Contracts.Dispatching;
using SchoolShuttleBus.Domain.Entities;
using SchoolShuttleBus.Domain.Shared;
using SchoolShuttleBus.Infrastructure.Persistence;

namespace SchoolShuttleBus.Infrastructure.Admin;

internal sealed class AdminService(
    SchoolShuttleBusDbContext dbContext,
    DispatchConflictDetector dispatchConflictDetector) : IAdminService
{
    public async Task<Result<DispatchOverrideResponse>> CreateDispatchOverrideAsync(CreateDispatchOverrideRequest request, CancellationToken cancellationToken)
    {
        var existing = await dbContext.DispatchOverrides
            .Where(item => item.StudentId == request.StudentId && item.Status == DispatchStatus.Active)
            .Select(item => new DispatchOverrideWindow(item.StudentId, item.Date, item.Direction, item.RouteId))
            .ToArrayAsync(cancellationToken);

        var candidate = new DispatchOverrideWindow(request.StudentId, request.Date, request.Direction, request.RouteId);
        var conflict = dispatchConflictDetector.Detect(existing, candidate);
        if (conflict.HasConflict)
        {
            return Result<DispatchOverrideResponse>.Fail(conflict.Reason ?? "Dispatch conflict detected.");
        }

        var entity = new DispatchOverride
        {
            StudentId = request.StudentId,
            RouteId = request.RouteId,
            Date = request.Date,
            Direction = request.Direction
        };

        dbContext.DispatchOverrides.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<DispatchOverrideResponse>.Ok(new DispatchOverrideResponse(entity.Id, entity.StudentId, entity.RouteId, entity.Date, entity.Direction, entity.Status));
    }

    public async Task<Result<ReportExportResponse>> CreateReportAsync(CreateReportRequest request, Guid actorUserId, CancellationToken cancellationToken)
    {
        var content = request.ReportType switch
        {
            ReportType.WeeklyRegistrations => await BuildRegistrationCsvAsync(request.StartDate, request.EndDate, cancellationToken),
            ReportType.AttendanceSummary => await BuildAttendanceCsvAsync(request.StartDate, request.EndDate, cancellationToken),
            ReportType.NotificationHistory => await BuildNotificationCsvAsync(cancellationToken),
            _ => "Unsupported report type"
        };

        var export = new ReportExport
        {
            ReportType = request.ReportType,
            ExportFormat = request.ExportFormat,
            FileName = $"{request.ReportType}-{DateTime.UtcNow:yyyyMMddHHmmss}.csv",
            ContentType = "text/csv",
            FiltersJson = JsonSerializer.Serialize(new { request.StartDate, request.EndDate }),
            Content = Encoding.UTF8.GetBytes(content),
            CreatedByUserId = actorUserId
        };

        dbContext.ReportExports.Add(export);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<ReportExportResponse>.Ok(new ReportExportResponse(
            export.Id,
            export.FileName,
            export.ContentType,
            export.ReportType,
            export.ExportFormat,
            export.CreatedAtUtc));
    }

    public async Task<Result<(byte[] Content, string ContentType, string FileName)>> GetReportContentAsync(Guid reportId, CancellationToken cancellationToken)
    {
        var report = await dbContext.ReportExports.SingleOrDefaultAsync(export => export.Id == reportId, cancellationToken);
        if (report is null)
        {
            return Result<(byte[] Content, string ContentType, string FileName)>.Fail("Report not found.");
        }

        return Result<(byte[] Content, string ContentType, string FileName)>.Ok((report.Content, report.ContentType, report.FileName));
    }

    private async Task<string> BuildRegistrationCsvAsync(DateOnly? startDate, DateOnly? endDate, CancellationToken cancellationToken)
    {
        var rows = await dbContext.RideRegistrations
            .Include(registration => registration.Student)
            .Where(registration => (!startDate.HasValue || registration.Date >= startDate.Value) &&
                                   (!endDate.HasValue || registration.Date <= endDate.Value))
            .OrderBy(registration => registration.Date)
            .Select(registration => $"{registration.Date:yyyy-MM-dd},{registration.Student.FullName},{registration.Direction},{registration.IsRegistered},{registration.IsPresent}")
            .ToArrayAsync(cancellationToken);

        return "Date,StudentName,Direction,IsRegistered,IsPresent\n" + string.Join('\n', rows);
    }

    private async Task<string> BuildAttendanceCsvAsync(DateOnly? startDate, DateOnly? endDate, CancellationToken cancellationToken)
    {
        var rows = await dbContext.AttendanceRecords
            .Include(record => record.Student)
            .Include(record => record.AttendanceSession)
            .Where(record => (!startDate.HasValue || record.AttendanceSession.Date >= startDate.Value) &&
                             (!endDate.HasValue || record.AttendanceSession.Date <= endDate.Value))
            .Select(record => $"{record.AttendanceSession.Date:yyyy-MM-dd},{record.Student.FullName},{record.Status}")
            .ToArrayAsync(cancellationToken);

        return "Date,StudentName,Status\n" + string.Join('\n', rows);
    }

    private async Task<string> BuildNotificationCsvAsync(CancellationToken cancellationToken)
    {
        var rows = await dbContext.NotificationDeliveries
            .OrderByDescending(delivery => delivery.CreatedAtUtc)
            .Select(delivery => $"{delivery.CreatedAtUtc:yyyy-MM-dd HH:mm:ss},{delivery.RecipientEmail},{delivery.Status}")
            .ToArrayAsync(cancellationToken);

        return "CreatedAtUtc,RecipientEmail,Status\n" + string.Join('\n', rows);
    }
}
