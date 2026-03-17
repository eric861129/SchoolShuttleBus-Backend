using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class StudentGuardianLink : AuditableEntity
{
    public Guid StudentId { get; set; }

    public Student Student { get; set; } = null!;

    public Guid GuardianId { get; set; }

    public Guardian Guardian { get; set; } = null!;

    public bool IsPrimaryContact { get; set; }
}
