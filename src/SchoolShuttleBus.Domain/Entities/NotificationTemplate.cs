using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class NotificationTemplate : AuditableEntity
{
    public string TemplateName { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;
}
