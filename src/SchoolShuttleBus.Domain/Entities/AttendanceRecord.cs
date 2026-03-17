using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 記錄學生在點名場次中的出缺席結果。
/// </summary>
public sealed class AttendanceRecord : AuditableEntity
{
    /// <summary>
    /// 所屬點名場次識別碼。
    /// </summary>
    public Guid AttendanceSessionId { get; set; }

    /// <summary>
    /// 所屬點名場次。
    /// </summary>
    public AttendanceSession AttendanceSession { get; set; } = null!;

    /// <summary>
    /// 學生識別碼。
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// 對應學生資料。
    /// </summary>
    public Student Student { get; set; } = null!;

    /// <summary>
    /// 點名狀態。
    /// </summary>
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Pending;

    /// <summary>
    /// 點名當下保留的緊急聯絡電話快照。
    /// </summary>
    public string EmergencyPhoneSnapshot { get; set; } = string.Empty;
}
