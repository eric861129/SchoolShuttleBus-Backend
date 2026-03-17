using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class AttendanceSession : AuditableEntity
{
    public Guid RouteId { get; set; }

    public Route Route { get; set; } = null!;

    public DateOnly Date { get; set; }

    public TripDirection Direction { get; set; }

    public bool IsCompleted { get; set; }

    public Guid CreatedByStaffProfileId { get; set; }

    public StaffProfile CreatedByStaffProfile { get; set; } = null!;

    public ICollection<AttendanceRecord> Records { get; set; } = new List<AttendanceRecord>();
}
