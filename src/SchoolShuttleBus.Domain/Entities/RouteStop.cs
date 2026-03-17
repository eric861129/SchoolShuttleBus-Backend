using SchoolShuttleBus.Domain.Common;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 定義路線上的停靠站資訊。
/// </summary>
public sealed class RouteStop : AuditableEntity
{
    /// <summary>
    /// 所屬路線識別碼。
    /// </summary>
    public Guid RouteId { get; set; }

    /// <summary>
    /// 所屬路線資料。
    /// </summary>
    public Route Route { get; set; } = null!;

    /// <summary>
    /// 停靠順序。
    /// </summary>
    public int Sequence { get; set; }

    /// <summary>
    /// 站點名稱。
    /// </summary>
    public string StopName { get; set; } = string.Empty;

    /// <summary>
    /// 站點地址。
    /// </summary>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// 交接聯絡人姓名。
    /// </summary>
    public string? HandoffContactName { get; set; }

    /// <summary>
    /// 交接聯絡人電話。
    /// </summary>
    public string? HandoffContactPhone { get; set; }
}
