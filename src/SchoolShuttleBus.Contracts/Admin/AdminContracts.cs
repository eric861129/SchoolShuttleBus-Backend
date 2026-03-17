using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Contracts.Admin;

public sealed record CreateDispatchOverrideRequest(Guid StudentId, Guid RouteId, DateOnly Date, TripDirection Direction);

public sealed record DispatchOverrideResponse(Guid DispatchOverrideId, Guid StudentId, Guid RouteId, DateOnly Date, TripDirection Direction, DispatchStatus Status);

public sealed record CreateBroadcastRequest(BroadcastAudience Audience, string Subject, string Body);

public sealed record BroadcastResponse(Guid BroadcastAnnouncementId, Guid NotificationJobId, int DeliveryCount);

public sealed record CreateReportRequest(ReportType ReportType, ExportFormat ExportFormat, DateOnly? StartDate, DateOnly? EndDate);

public sealed record ReportExportResponse(Guid ReportExportId, string FileName, string ContentType, ReportType ReportType, ExportFormat ExportFormat, DateTimeOffset CreatedAtUtc);
