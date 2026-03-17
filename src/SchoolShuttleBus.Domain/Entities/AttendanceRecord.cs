using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class AttendanceRecord : AuditableEntity
{
    public Guid AttendanceSessionId { get; set; }

    public AttendanceSession AttendanceSession { get; set; } = null!;

    public Guid StudentId { get; set; }

    public Student Student { get; set; } = null!;

    public AttendanceStatus Status { get; set; } = AttendanceStatus.Pending;

    public string EmergencyPhoneSnapshot { get; set; } = string.Empty;
}
