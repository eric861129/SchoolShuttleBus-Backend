using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class RouteStop : AuditableEntity
{
    public Guid RouteId { get; set; }

    public Route Route { get; set; } = null!;

    public int Sequence { get; set; }

    public string StopName { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string? HandoffContactName { get; set; }

    public string? HandoffContactPhone { get; set; }
}
