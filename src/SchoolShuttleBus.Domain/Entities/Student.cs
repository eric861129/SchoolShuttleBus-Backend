using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class Student : AuditableEntity
{
    public Guid UserId { get; set; }

    public string StudentNumber { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public StudentStage Stage { get; set; }

    public string GradeLabel { get; set; } = string.Empty;

    public Guid? DefaultRouteId { get; set; }

    public Route? DefaultRoute { get; set; }

    public ICollection<StudentGuardianLink> GuardianLinks { get; set; } = new List<StudentGuardianLink>();
}
