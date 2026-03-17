using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 儲存可重複使用的通知範本。
/// </summary>
public sealed class NotificationTemplate : AuditableEntity
{
    /// <summary>
    /// 範本名稱。
    /// </summary>
    public string TemplateName { get; set; } = string.Empty;

    /// <summary>
    /// 範本主旨。
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// 範本內容。
    /// </summary>
    public string Body { get; set; } = string.Empty;
}
