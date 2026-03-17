using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolShuttleBus.Application.Auth;
using SchoolShuttleBus.Application.Notifications;
using SchoolShuttleBus.Contracts.Notifications;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Api.Controllers;

/// <summary>
/// 提供通知管理相關端點，包含手動執行提醒與查詢通知歷程。
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class NotificationsController(
    INotificationService notificationService,
    ICurrentUserAccessor currentUserAccessor) : ControllerBase
{
    /// <summary>
    /// 不受背景排程限制，手動立即執行提醒作業。
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost("reminders/run")]
    public async Task<ActionResult<ReminderRunResponse>> RunRemindersAsync(CancellationToken cancellationToken)
    {
        var result = await notificationService.RunRemindersAsync(currentUserAccessor.UserId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 取得最近的通知寄送歷程紀錄。
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpGet("history")]
    public async Task<ActionResult<IReadOnlyCollection<NotificationDeliveryResponse>>> GetHistoryAsync(CancellationToken cancellationToken)
    {
        return Ok(await notificationService.GetHistoryAsync(cancellationToken));
    }
}
