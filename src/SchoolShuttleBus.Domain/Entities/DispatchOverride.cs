using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class DispatchOverride : AuditableEntity
{
    public Guid StudentId { get; set; }

    public Student Student { get; set; } = null!;

    public Guid RouteId { get; set; }

    public Route Route { get; set; } = null!;

    public DateOnly Date { get; set; }

    public TripDirection Direction { get; set; }

    public DispatchStatus Status { get; set; } = DispatchStatus.Active;
}
