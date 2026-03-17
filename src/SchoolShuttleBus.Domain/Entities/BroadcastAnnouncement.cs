using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class BroadcastAnnouncement : AuditableEntity
{
    public BroadcastAudience Audience { get; set; }

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public Guid CreatedByUserId { get; set; }
}
