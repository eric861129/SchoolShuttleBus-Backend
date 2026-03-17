using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolShuttleBus.Application.Attendance;
using SchoolShuttleBus.Contracts.Attendance;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Api.Controllers;

/// <summary>
/// 提供車上點名流程使用的點名場次與點名紀錄相關端點。
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class AttendanceController(IAttendanceService attendanceService) : ControllerBase
{
    /// <summary>
    /// 取得目前使用者可查看的點名場次列表。
    /// </summary>
    [HttpGet("sessions")]
    public async Task<ActionResult<IReadOnlyCollection<AttendanceSessionResponse>>> GetSessionsAsync(CancellationToken cancellationToken)
        => Ok(await attendanceService.GetSessionsAsync(cancellationToken));

    /// <summary>
    /// 依據路線搭乘登記資料建立一筆點名場次。
    /// </summary>
    [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Administrator}")]
    [HttpPost("sessions")]
    public async Task<ActionResult<AttendanceSessionResponse>> CreateSessionAsync([FromBody] CreateAttendanceSessionRequest request, CancellationToken cancellationToken)
    {
        var result = await attendanceService.CreateSessionAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 更新單一學生的點名狀態。
    /// </summary>
    [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Administrator}")]
    [HttpPatch("records/{recordId:guid}")]
    public async Task<ActionResult<AttendanceRecordResponse>> UpdateRecordAsync(Guid recordId, [FromBody] UpdateAttendanceRecordRequest request, CancellationToken cancellationToken)
    {
        var result = await attendanceService.UpdateRecordAsync(recordId, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 將指定點名場次標記為已完成。
    /// </summary>
    [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Administrator}")]
    [HttpPost("sessions/{sessionId:guid}/complete")]
    public async Task<ActionResult<AttendanceSessionResponse>> CompleteSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        var result = await attendanceService.CompleteSessionAsync(sessionId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }
}
