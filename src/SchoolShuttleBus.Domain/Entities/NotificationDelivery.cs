using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class NotificationDelivery : AuditableEntity
{
    public Guid NotificationJobId { get; set; }

    public NotificationJob NotificationJob { get; set; } = null!;

    public Guid? RecipientUserId { get; set; }

    public string RecipientEmail { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    public DateTimeOffset? SentAtUtc { get; set; }

    public string? ErrorMessage { get; set; }
}
