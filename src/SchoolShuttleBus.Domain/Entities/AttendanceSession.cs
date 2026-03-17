using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 表示某條路線在特定日期與方向的一次點名場次。
/// </summary>
public sealed class AttendanceSession : AuditableEntity
{
    /// <summary>
    /// 路線識別碼。
    /// </summary>
    public Guid RouteId { get; set; }

    /// <summary>
    /// 對應路線。
    /// </summary>
    public Route Route { get; set; } = null!;

    /// <summary>
    /// 點名日期。
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// 行車方向。
    /// </summary>
    public TripDirection Direction { get; set; }

    /// <summary>
    /// 是否已完成點名。
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// 建立場次的教職員識別碼。
    /// </summary>
    public Guid CreatedByStaffProfileId { get; set; }

    /// <summary>
    /// 建立場次的教職員資料。
    /// </summary>
    public StaffProfile CreatedByStaffProfile { get; set; } = null!;

    /// <summary>
    /// 場次內的點名紀錄集合。
    /// </summary>
    public ICollection<AttendanceRecord> Records { get; set; } = new List<AttendanceRecord>();
}
