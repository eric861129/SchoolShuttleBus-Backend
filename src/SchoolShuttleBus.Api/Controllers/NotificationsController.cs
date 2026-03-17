using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolShuttleBus.Application.Auth;
using SchoolShuttleBus.Application.Notifications;
using SchoolShuttleBus.Contracts.Notifications;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Api.Controllers;

/// <summary>
/// Notification management endpoints, including manual reminder runs and history lookup.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class NotificationsController(
    INotificationService notificationService,
    ICurrentUserAccessor currentUserAccessor) : ControllerBase
{
    /// <summary>
    /// Manually runs the reminder job regardless of the background schedule.
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost("reminders/run")]
    public async Task<ActionResult> RunRemindersAsync(CancellationToken cancellationToken)
    {
        var result = await notificationService.RunRemindersAsync(currentUserAccessor.UserId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// Returns the most recent email delivery history entries.
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpGet("history")]
    public async Task<ActionResult<IReadOnlyCollection<NotificationDeliveryResponse>>> GetHistoryAsync(CancellationToken cancellationToken)
    {
        return Ok(await notificationService.GetHistoryAsync(cancellationToken));
    }
}
