using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class StaffProfile : AuditableEntity
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public bool CanManageAllRoutes { get; set; }

    public ICollection<RouteAssignment> RouteAssignments { get; set; } = new List<RouteAssignment>();
}
