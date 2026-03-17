using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

public sealed class Guardian : AuditableEntity
{
    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public ICollection<StudentGuardianLink> StudentLinks { get; set; } = new List<StudentGuardianLink>();
}
