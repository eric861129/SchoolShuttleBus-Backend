namespace SchoolShuttleBus.Infrastructure.Auth;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "SchoolShuttleBus";

    public string Audience { get; set; } = "SchoolShuttleBus.Clients";

    public string SigningKey { get; set; } = "dev-signing-key-change-me-1234567890";

    public int AccessTokenMinutes { get; set; } = 120;

    public int RefreshTokenDays { get; set; } = 14;
}
