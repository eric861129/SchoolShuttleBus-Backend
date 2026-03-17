using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 表示一批待發送或已發送的通知工作。
/// </summary>
public sealed class NotificationJob : AuditableEntity
{
    /// <summary>
    /// 通知工作類型。
    /// </summary>
    public string JobType { get; set; } = string.Empty;

    /// <summary>
    /// 通知發送管道。
    /// </summary>
    public NotificationChannel Channel { get; set; } = NotificationChannel.Email;

    /// <summary>
    /// 通知主旨。
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// 通知內容。
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// 建立通知的使用者識別碼。
    /// </summary>
    public Guid? CreatedByUserId { get; set; }

    /// <summary>
    /// 各收件人的發送結果集合。
    /// </summary>
    public ICollection<NotificationDelivery> Deliveries { get; set; } = new List<NotificationDelivery>();
}
