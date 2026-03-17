using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SchoolShuttleBus.Application.Auth;
using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Contracts.Auth;
using SchoolShuttleBus.Infrastructure.Persistence;

namespace SchoolShuttleBus.Infrastructure.Auth;

public sealed class AuthService(
    UserManager<AppUser> userManager,
    SchoolShuttleBusDbContext dbContext,
    ITokenFactory tokenFactory,
    IOptions<JwtOptions> jwtOptions) : IAuthService
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<Result<TokenResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result<TokenResponse>.Fail("Invalid email or password.");
        }

        return Result<TokenResponse>.Ok(await CreateTokenEnvelopeAsync(user, cancellationToken));
    }

    public async Task<Result<TokenResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshToken = await dbContext.RefreshTokens
            .Include(token => token.User)
            .SingleOrDefaultAsync(token => token.Token == request.RefreshToken, cancellationToken);

        if (refreshToken is null || !refreshToken.IsActive)
        {
            return Result<TokenResponse>.Fail("Refresh token is invalid or expired.");
        }

        refreshToken.RevokedAtUtc = DateTimeOffset.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<TokenResponse>.Ok(await CreateTokenEnvelopeAsync(refreshToken.User, cancellationToken));
    }

    public async Task LogoutAsync(Guid userId, CancellationToken cancellationToken)
    {
        var activeTokens = await dbContext.RefreshTokens
            .Where(token => token.UserId == userId && token.RevokedAtUtc == null)
            .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
        {
            token.RevokedAtUtc = DateTimeOffset.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result<CurrentUserResponse>> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.SingleOrDefaultAsync(candidate => candidate.Id == userId, cancellationToken);
        if (user is null)
        {
            return Result<CurrentUserResponse>.Fail("User not found.");
        }

        var roles = await userManager.GetRolesAsync(user);
        return Result<CurrentUserResponse>.Ok(new CurrentUserResponse(user.Id, user.Email ?? string.Empty, roles.ToArray()));
    }

    private async Task<TokenResponse> CreateTokenEnvelopeAsync(AppUser user, CancellationToken cancellationToken)
    {
        var roles = await userManager.GetRolesAsync(user);
        var expiresAtUtc = DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.AccessTokenMinutes);
        var roleArray = roles.ToArray();
        var accessToken = tokenFactory.CreateAccessToken(user.Id, user.Email ?? string.Empty, roleArray, expiresAtUtc);
        var refreshTokenValue = tokenFactory.CreateRefreshToken();

        dbContext.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenValue,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(_jwtOptions.RefreshTokenDays)
        });

        await dbContext.SaveChangesAsync(cancellationToken);

        return new TokenResponse(user.Id, user.Email ?? string.Empty, accessToken, refreshTokenValue, expiresAtUtc);
    }
}
