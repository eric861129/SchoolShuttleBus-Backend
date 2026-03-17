using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolShuttleBus.Application.Auth;
using SchoolShuttleBus.Contracts.Auth;

namespace SchoolShuttleBus.Api.Controllers;

/// <summary>
/// 提供示範帳號登入、JWT 權杖管理與目前使用者資訊查詢的驗證相關端點。
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(IAuthService authService, ICurrentUserAccessor currentUserAccessor) : ControllerBase
{
    /// <summary>
    /// 使用帳號與密碼換取 access token 與 refresh token。
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> LoginAsync([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 使用 refresh token 換發新的登入權杖組。
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponse>> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.RefreshAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Unauthorized(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 註銷目前使用者所有仍有效的 refresh token。
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
    /// 取得目前登入使用者的基本身分資訊與角色列表。
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

    /// <summary>
    /// 回傳前端啟動所需的使用者上下文資料，包含可存取學生與教職員設定資訊。
    /// </summary>
    [Authorize]
    [HttpGet("context")]
    public async Task<ActionResult<CurrentUserContextResponse>> ContextAsync(CancellationToken cancellationToken)
    {
        if (currentUserAccessor.UserId is not { } userId)
        {
            return Unauthorized();
        }

        var result = await authService.GetCurrentUserContextAsync(userId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }
}
