using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 記錄學生在特定日期與方向的搭車登記結果。
/// </summary>
public sealed class RideRegistration : AuditableEntity
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
    /// 搭車日期。
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// 行車方向。
    /// </summary>
    public TripDirection Direction { get; set; }

    /// <summary>
    /// 是否完成搭車登記。
    /// </summary>
    public bool IsRegistered { get; set; }

    /// <summary>
    /// 實際是否到場搭乘。
    /// </summary>
    public bool IsPresent { get; set; }

    /// <summary>
    /// 指派路線識別碼。
    /// </summary>
    public Guid? RouteId { get; set; }

    /// <summary>
    /// 指派路線資料。
    /// </summary>
    public Route? Route { get; set; }
}
