using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class ReportExport : AuditableEntity
{
    public ReportType ReportType { get; set; }

    public ExportFormat ExportFormat { get; set; } = ExportFormat.Csv;

    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = "text/csv";

    public string FiltersJson { get; set; } = "{}";

    public byte[] Content { get; set; } = [];

    public Guid CreatedByUserId { get; set; }
}
