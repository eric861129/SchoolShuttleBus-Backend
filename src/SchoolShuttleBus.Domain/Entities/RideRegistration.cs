using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class RideRegistration : AuditableEntity
{
    public Guid StudentId { get; set; }

    public Student Student { get; set; } = null!;

    public DateOnly Date { get; set; }

    public TripDirection Direction { get; set; }

    public bool IsRegistered { get; set; }

    public bool IsPresent { get; set; }

    public Guid? RouteId { get; set; }

    public Route? Route { get; set; }
}
