using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Contracts.Auth;

namespace SchoolShuttleBus.Application.Auth;

public interface IAuthService
{
    Task<Result<TokenResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    Task<Result<TokenResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken);

    Task LogoutAsync(Guid userId, CancellationToken cancellationToken);

    Task<Result<CurrentUserResponse>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken);

    Task<Result<CurrentUserContextResponse>> GetCurrentUserContextAsync(Guid userId, CancellationToken cancellationToken);
}
