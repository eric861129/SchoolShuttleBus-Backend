using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 連結路線與負責教職員的指派關係。
/// </summary>
public sealed class RouteAssignment : AuditableEntity
{
    /// <summary>
    /// 路線識別碼。
    /// </summary>
    public Guid RouteId { get; set; }

    /// <summary>
    /// 對應路線資料。
    /// </summary>
    public Route Route { get; set; } = null!;

    /// <summary>
    /// 教職員識別碼。
    /// </summary>
    public Guid StaffProfileId { get; set; }

    /// <summary>
    /// 對應教職員資料。
    /// </summary>
    public StaffProfile StaffProfile { get; set; } = null!;
}
