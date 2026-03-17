using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 表示教職員個人檔案與路線管理能力。
/// </summary>
public sealed class StaffProfile : AuditableEntity
{
    /// <summary>
    /// 對應登入使用者識別碼。
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 工號。
    /// </summary>
    public string EmployeeNumber { get; set; } = string.Empty;

    /// <summary>
    /// 教職員姓名。
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// 教職員聯絡電話。
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// 是否可管理全部路線。
    /// </summary>
    public bool CanManageAllRoutes { get; set; }

    /// <summary>
    /// 教職員被指派的路線集合。
    /// </summary>
    public ICollection<RouteAssignment> RouteAssignments { get; set; } = new List<RouteAssignment>();
}
