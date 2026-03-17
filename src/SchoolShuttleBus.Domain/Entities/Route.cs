using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 定義校車路線主檔。
/// </summary>
public sealed class Route : AuditableEntity
{
    /// <summary>
    /// 路線名稱。
    /// </summary>
    public string RouteName { get; set; } = string.Empty;

    /// <summary>
    /// 路線類型。
    /// </summary>
    public RouteType RouteType { get; set; }

    /// <summary>
    /// 行車方向。
    /// </summary>
    public TripDirection Direction { get; set; }

    /// <summary>
    /// 所屬校區名稱。
    /// </summary>
    public string CampusName { get; set; } = string.Empty;

    /// <summary>
    /// 是否啟用。
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// 路線停靠站集合。
    /// </summary>
    public ICollection<RouteStop> Stops { get; set; } = new List<RouteStop>();

    /// <summary>
    /// 路線指派的教職員集合。
    /// </summary>
    public ICollection<RouteAssignment> Assignments { get; set; } = new List<RouteAssignment>();
}
