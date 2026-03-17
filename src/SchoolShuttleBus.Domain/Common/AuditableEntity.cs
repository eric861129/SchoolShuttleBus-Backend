namespace SchoolShuttleBus.Domain.Common;

/// <summary>
/// 定義所有可稽核實體的共用欄位。
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>
    /// 實體唯一識別碼。
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// 建立時間（UTC）。
    /// </summary>
    public DateTimeOffset CreatedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// 最後更新時間（UTC）。
    /// </summary>
    public DateTimeOffset? UpdatedAtUtc { get; set; }
}
