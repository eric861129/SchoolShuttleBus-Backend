namespace SchoolShuttleBus.Application.Auth;

public interface ITokenFactory
{
    string CreateAccessToken(Guid userId, string email, IReadOnlyCollection<string> roles, DateTimeOffset expiresAtUtc);

    string CreateRefreshToken();
}
