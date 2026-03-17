using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolShuttleBus.Application.Registrations;
using SchoolShuttleBus.Contracts.Registrations;

namespace SchoolShuttleBus.Api.Controllers;

/// <summary>
/// Weekly shuttle registration endpoints for parents, students, and administrators.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class RegistrationsController(IRegistrationService registrationService) : ControllerBase
{
    /// <summary>
    /// Returns a student's registration grid for the requested week.
    /// </summary>
    [HttpGet("weeks/{weekStart}")]
    public async Task<ActionResult<WeeklyRegistrationResponse>> GetWeekAsync(DateOnly weekStart, [FromQuery] Guid studentId, CancellationToken cancellationToken)
    {
        var result = await registrationService.GetWeekAsync(studentId, weekStart, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// Replaces a student's weekly registration selections.
    /// </summary>
    [HttpPut("weeks/{weekStart}")]
    public async Task<ActionResult<WeeklyRegistrationResponse>> UpdateWeekAsync(DateOnly weekStart, [FromBody] UpdateWeeklyRegistrationRequest request, CancellationToken cancellationToken)
    {
        var normalizedRequest = request with { WeekStart = weekStart };
        var result = await registrationService.UpdateWeekAsync(normalizedRequest, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// Copies the previous week's registration pattern into the requested week.
    /// </summary>
    [HttpPost("weeks/{weekStart}/copy-last-week")]
    public async Task<ActionResult<WeeklyRegistrationResponse>> CopyLastWeekAsync(DateOnly weekStart, [FromQuery] Guid studentId, CancellationToken cancellationToken)
    {
        var result = await registrationService.CopyLastWeekAsync(studentId, weekStart, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// Returns the aggregate registration and attendance summary for a student.
    /// </summary>
    [HttpGet("students/{studentId}/summary")]
    public async Task<ActionResult<StudentRegistrationSummaryResponse>> GetSummaryAsync(Guid studentId, CancellationToken cancellationToken)
    {
        var result = await registrationService.GetSummaryAsync(studentId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }
}
