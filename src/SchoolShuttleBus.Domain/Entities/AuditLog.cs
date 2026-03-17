using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class AuditLog : AuditableEntity
{
    public Guid? ActorUserId { get; set; }

    public string Action { get; set; } = string.Empty;

    public string EntityName { get; set; } = string.Empty;

    public Guid? EntityId { get; set; }

    public string MetadataJson { get; set; } = "{}";
}
