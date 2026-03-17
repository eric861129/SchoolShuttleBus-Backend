using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 表示學生主檔與預設搭乘設定。
/// </summary>
public sealed class Student : AuditableEntity
{
    /// <summary>
    /// 對應登入使用者識別碼。
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 學號。
    /// </summary>
    public string StudentNumber { get; set; } = string.Empty;

    /// <summary>
    /// 學生姓名。
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// 學生學制階段。
    /// </summary>
    public StudentStage Stage { get; set; }

    /// <summary>
    /// 年級文字標籤。
    /// </summary>
    public string GradeLabel { get; set; } = string.Empty;

    /// <summary>
    /// 預設路線識別碼。
    /// </summary>
    public Guid? DefaultRouteId { get; set; }

    /// <summary>
    /// 預設路線資料。
    /// </summary>
    public Route? DefaultRoute { get; set; }

    /// <summary>
    /// 關聯家長集合。
    /// </summary>
    public ICollection<StudentGuardianLink> GuardianLinks { get; set; } = new List<StudentGuardianLink>();
}
