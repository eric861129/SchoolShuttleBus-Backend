using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 建立學生與家長之間的關聯關係。
/// </summary>
public sealed class StudentGuardianLink : AuditableEntity
{
    /// <summary>
    /// 學生識別碼。
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// 對應學生資料。
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    /// 家長識別碼。
    /// </summary>
    public Guid GuardianId { get; set; }

    /// <summary>
    /// 對應家長資料。
    /// </summary>
    public Guardian Guardian { get; set; } = null!;

    /// <summary>
    /// 是否為主要聯絡人。
    /// </summary>
    public bool IsPrimaryContact { get; set; }
}
