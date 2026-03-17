using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 記錄系統內重要操作的稽核事件。
/// </summary>
public sealed class AuditLog : AuditableEntity
{
    /// <summary>
    /// 執行動作的使用者識別碼。
    /// </summary>
    public Guid? ActorUserId { get; set; }

    /// <summary>
    /// 動作名稱。
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// 目標實體名稱。
    /// </summary>
    public string EntityName { get; set; } = string.Empty;

    /// <summary>
    /// 目標實體識別碼。
    /// </summary>
    public Guid? EntityId { get; set; }

    /// <summary>
    /// 額外稽核資訊的 JSON 內容。
    /// </summary>
    public string MetadataJson { get; set; } = "{}";
}
