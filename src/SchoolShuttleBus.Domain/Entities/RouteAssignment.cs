using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class RouteAssignment : AuditableEntity
{
    public Guid RouteId { get; set; }

    public Route Route { get; set; } = null!;

    public Guid StaffProfileId { get; set; }

    public StaffProfile StaffProfile { get; set; } = null!;
}
