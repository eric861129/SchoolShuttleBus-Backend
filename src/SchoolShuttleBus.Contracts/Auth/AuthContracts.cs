namespace SchoolShuttleBus.Contracts.Auth;

public sealed record LoginRequest(string Account, string Password);

public sealed record RefreshTokenRequest(string RefreshToken);

public sealed record TokenResponse(Guid UserId, string Email, string AccessToken, string RefreshToken, DateTimeOffset ExpiresAtUtc);

public sealed record CurrentUserResponse(Guid UserId, string Email, IReadOnlyCollection<string> Roles);
