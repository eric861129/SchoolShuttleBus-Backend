using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 記錄報表匯出的輸出內容與條件。
/// </summary>
public sealed class ReportExport : AuditableEntity
{
    /// <summary>
    /// 報表類型。
    /// </summary>
    public ReportType ReportType { get; set; }

    /// <summary>
    /// 匯出格式。
    /// </summary>
    public ExportFormat ExportFormat { get; set; } = ExportFormat.Csv;

    /// <summary>
    /// 匯出檔名。
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 檔案內容類型。
    /// </summary>
    public string ContentType { get; set; } = "text/csv";

    /// <summary>
    /// 匯出條件的 JSON 內容。
    /// </summary>
    public string FiltersJson { get; set; } = "{}";

    /// <summary>
    /// 匯出檔案內容。
    /// </summary>
    public byte[] Content { get; set; } = [];

    /// <summary>
    /// 建立匯出的使用者識別碼。
    /// </summary>
    public Guid CreatedByUserId { get; set; }
}
