using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class NotificationJob : AuditableEntity
{
    public string JobType { get; set; } = string.Empty;

    public NotificationChannel Channel { get; set; } = NotificationChannel.Email;

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public Guid? CreatedByUserId { get; set; }

    public ICollection<NotificationDelivery> Deliveries { get; set; } = new List<NotificationDelivery>();
}
