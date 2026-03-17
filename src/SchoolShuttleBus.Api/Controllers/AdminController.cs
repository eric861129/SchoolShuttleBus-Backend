using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolShuttleBus.Application.Admin;
using SchoolShuttleBus.Application.Auth;
using SchoolShuttleBus.Application.Notifications;
using SchoolShuttleBus.Contracts.Admin;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Api.Controllers;

/// <summary>
/// 提供管理端使用的調度覆寫、公告通知與報表匯出相關端點。
/// </summary>
[ApiController]
[Authorize(Roles = RoleNames.Administrator)]
[Route("api/[controller]")]
public sealed class AdminController(
    IAdminService adminService,
    INotificationService notificationService,
    ICurrentUserAccessor currentUserAccessor) : ControllerBase
{
    /// <summary>
    /// 針對指定學生的單次搭乘行程建立調度覆寫設定。
    /// </summary>
    [HttpPost("dispatches")]
    public async Task<ActionResult<DispatchOverrideResponse>> CreateDispatchAsync([FromBody] CreateDispatchOverrideRequest request, CancellationToken cancellationToken)
    {
        var result = await adminService.CreateDispatchOverrideAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Conflict(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 對指定對象發送全站廣播通知。
    /// </summary>
    [HttpPost("broadcasts")]
    public async Task<ActionResult<BroadcastResponse>> CreateBroadcastAsync([FromBody] CreateBroadcastRequest request, CancellationToken cancellationToken)
    {
        if (currentUserAccessor.UserId is not { } userId)
        {
            return Unauthorized();
        }

        var result = await notificationService.CreateBroadcastAsync(request, userId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 產生報表檔案並保存，供後續下載使用。
    /// </summary>
    [HttpPost("reports")]
    public async Task<ActionResult<ReportExportResponse>> CreateReportAsync([FromBody] CreateReportRequest request, CancellationToken cancellationToken)
    {
        if (currentUserAccessor.UserId is not { } userId)
        {
            return Unauthorized();
        }

        var result = await adminService.CreateReportAsync(request, userId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 取得管理端表單所需的學生與教職員下拉選單資料。
    /// </summary>
    [HttpGet("lookups")]
    public async Task<ActionResult<AdminLookupsResponse>> GetLookupsAsync(CancellationToken cancellationToken)
    {
        return Ok(await adminService.GetLookupsAsync(cancellationToken));
    }

    /// <summary>
    /// 下載先前已產生的報表檔案內容。
    /// </summary>
    [HttpGet("reports/{reportId:guid}")]
    public async Task<IActionResult> DownloadReportAsync(Guid reportId, CancellationToken cancellationToken)
    {
        var result = await adminService.GetReportContentAsync(reportId, cancellationToken);
        return result.IsSuccess
            ? File(result.Value.Content, result.Value.ContentType, result.Value.FileName)
            : NotFound(new ProblemDetails { Title = result.Error });
    }
}
