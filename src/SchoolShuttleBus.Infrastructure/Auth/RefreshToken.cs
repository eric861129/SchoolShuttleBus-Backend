namespace SchoolShuttleBus.Infrastructure.Auth;

/// <summary>
/// 儲存使用者的重新整理權杖資訊。
/// </summary>
public sealed class RefreshToken
{
    /// <summary>
    /// 重新整理權杖識別碼。
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// 對應使用者識別碼。
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// 對應使用者資料。
    /// </summary>
    public AppUser User { get; set; } = null!;

    /// <summary>
    /// 實際權杖字串。
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// 權杖到期時間（UTC）。
    /// </summary>
    public DateTimeOffset ExpiresAtUtc { get; set; }

    /// <summary>
    /// 權杖撤銷時間（UTC）。
    /// </summary>
    public DateTimeOffset? RevokedAtUtc { get; set; }

    /// <summary>
    /// 代表權杖是否仍可使用。
    /// </summary>
    public bool IsActive => RevokedAtUtc is null && ExpiresAtUtc > DateTimeOffset.UtcNow;
}
