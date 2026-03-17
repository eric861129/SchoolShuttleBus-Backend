using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 記錄派車覆寫設定，用於臨時調整學生的派車結果。
/// </summary>
public sealed class DispatchOverride : AuditableEntity
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
    /// 指派路線識別碼。
    /// </summary>
    public Guid RouteId { get; set; }

    /// <summary>
    /// 指派路線資料。
    /// </summary>
    public Route Route { get; set; } = null!;

    /// <summary>
    /// 覆寫生效日期。
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// 行車方向。
    /// </summary>
    public TripDirection Direction { get; set; }

    /// <summary>
    /// 覆寫狀態。
    /// </summary>
    public DispatchStatus Status { get; set; } = DispatchStatus.Active;
}
