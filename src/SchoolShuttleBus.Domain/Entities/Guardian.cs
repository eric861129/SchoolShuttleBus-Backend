using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 表示學生的家長或監護人資料。
/// </summary>
public sealed class Guardian : AuditableEntity
{
    /// <summary>
    /// 對應登入使用者識別碼。
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 家長姓名。
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// 家長手機號碼。
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 家長電子郵件。
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// 關聯的學生清單。
    /// </summary>
    public ICollection<StudentGuardianLink> StudentLinks { get; set; } = new List<StudentGuardianLink>();
}
