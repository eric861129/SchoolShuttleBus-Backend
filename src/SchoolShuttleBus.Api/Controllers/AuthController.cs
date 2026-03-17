using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolShuttleBus.Application.Auth;
using SchoolShuttleBus.Contracts.Auth;

namespace SchoolShuttleBus.Api.Controllers;

/// <summary>
/// Authentication endpoints for demo accounts and JWT session management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IAuthService authService, ICurrentUserAccessor currentUserAccessor) : ControllerBase
{
    /// <summary>
    /// Exchanges an account/password pair for an access token and refresh token.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// Rotates a refresh token and returns a fresh token envelope.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponse>> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RefreshAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// Revokes all active refresh tokens for the current user.
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
    {
        if (currentUserAccessor.UserId is not { } userId)
        {
            return Unauthorized();
        }

        await authService.LogoutAsync(userId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Returns the authenticated user's basic identity and role list.
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<CurrentUserResponse>> MeAsync(CancellationToken cancellationToken)
    {
        if (currentUserAccessor.UserId is not { } userId)
        {
            return Unauthorized();
        }

        var result = await authService.GetCurrentUserAsync(userId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }
}
