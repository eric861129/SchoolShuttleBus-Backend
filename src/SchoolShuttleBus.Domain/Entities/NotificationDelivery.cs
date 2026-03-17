using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 記錄單一通知工作對個別收件人的發送結果。
/// </summary>
public sealed class NotificationDelivery : AuditableEntity
{
    /// <summary>
    /// 所屬通知工作識別碼。
    /// </summary>
    public Guid NotificationJobId { get; set; }

    /// <summary>
    /// 所屬通知工作。
    /// </summary>
    public NotificationJob NotificationJob { get; set; } = null!;

    /// <summary>
    /// 收件者使用者識別碼。
    /// </summary>
    public Guid? RecipientUserId { get; set; }

    /// <summary>
    /// 收件者電子郵件。
    /// </summary>
    public string RecipientEmail { get; set; } = string.Empty;

    /// <summary>
    /// 發送狀態。
    /// </summary>
    public string Status { get; set; } = "Pending";

    /// <summary>
    /// 實際發送時間（UTC）。
    /// </summary>
    public DateTimeOffset? SentAtUtc { get; set; }

    /// <summary>
    /// 發送失敗訊息。
    /// </summary>
    public string? ErrorMessage { get; set; }
}
