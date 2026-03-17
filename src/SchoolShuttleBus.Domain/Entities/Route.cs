using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class Route : AuditableEntity
{
    public string RouteName { get; set; } = string.Empty;

    public RouteType RouteType { get; set; }

    public TripDirection Direction { get; set; }

    public string CampusName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public ICollection<RouteStop> Stops { get; set; } = new List<RouteStop>();

    public ICollection<RouteAssignment> Assignments { get; set; } = new List<RouteAssignment>();
}
