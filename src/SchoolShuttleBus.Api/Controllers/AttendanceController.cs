using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolShuttleBus.Application.Attendance;
using SchoolShuttleBus.Contracts.Attendance;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Api.Controllers;

/// <summary>
/// Attendance session and record endpoints for onboard roll call workflows.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class AttendanceController(IAttendanceService attendanceService) : ControllerBase
{
    /// <summary>
    /// Returns the attendance sessions visible to the current user.
    /// </summary>
    [HttpGet("sessions")]
    public async Task<ActionResult<IReadOnlyCollection<AttendanceSessionResponse>>> GetSessionsAsync(CancellationToken cancellationToken)
        => Ok(await attendanceService.GetSessionsAsync(cancellationToken));

    /// <summary>
    /// Creates a roll call session from route registrations.
    /// </summary>
    [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Administrator}")]
    [HttpPost("sessions")]
    public async Task<ActionResult<AttendanceSessionResponse>> CreateSessionAsync([FromBody] CreateAttendanceSessionRequest request, CancellationToken cancellationToken)
    {
        var result = await attendanceService.CreateSessionAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// Updates a single student's attendance status.
    /// </summary>
    [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Administrator}")]
    [HttpPatch("records/{recordId:guid}")]
    public async Task<ActionResult<AttendanceRecordResponse>> UpdateRecordAsync(Guid recordId, [FromBody] UpdateAttendanceRecordRequest request, CancellationToken cancellationToken)
    {
        var result = await attendanceService.UpdateRecordAsync(recordId, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// Marks an attendance session as completed.
    /// </summary>
    [Authorize(Roles = $"{RoleNames.Teacher},{RoleNames.Administrator}")]
    [HttpPost("sessions/{sessionId:guid}/complete")]
    public async Task<ActionResult<AttendanceSessionResponse>> CompleteSessionAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        var result = await attendanceService.CompleteSessionAsync(sessionId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }
}
