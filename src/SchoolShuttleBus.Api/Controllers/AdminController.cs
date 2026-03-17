using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolShuttleBus.Application.Admin;
using SchoolShuttleBus.Application.Auth;
using SchoolShuttleBus.Application.Notifications;
using SchoolShuttleBus.Contracts.Admin;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Api.Controllers;

/// <summary>
/// Administrative endpoints for dispatch overrides, broadcasts, and report exports.
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
    /// Creates a route override for a specific student trip.
    /// </summary>
    [HttpPost("dispatches")]
    public async Task<ActionResult<DispatchOverrideResponse>> CreateDispatchAsync([FromBody] CreateDispatchOverrideRequest request, CancellationToken cancellationToken)
    {
        var result = await adminService.CreateDispatchOverrideAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Conflict(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// Sends a global email broadcast to the selected audience.
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
    /// Generates a report file and stores it for later download.
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
    /// Returns students and staff profiles used to populate administrative SPA form options.
    /// </summary>
    [HttpGet("lookups")]
    public async Task<ActionResult<AdminLookupsResponse>> GetLookupsAsync(CancellationToken cancellationToken)
    {
        return Ok(await adminService.GetLookupsAsync(cancellationToken));
    }

    /// <summary>
    /// Downloads the binary content of a previously generated report.
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
