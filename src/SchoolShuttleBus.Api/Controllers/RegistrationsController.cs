using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolShuttleBus.Application.Registrations;
using SchoolShuttleBus.Contracts.Registrations;

namespace SchoolShuttleBus.Api.Controllers;

/// <summary>
/// 提供家長、學生與管理者操作的每週校車搭乘登記相關端點。
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class RegistrationsController(IRegistrationService registrationService) : ControllerBase
{
    /// <summary>
    /// 取得指定學生於目標週次的搭乘登記表格資料。
    /// </summary>
    [HttpGet("weeks/{weekStart}")]
    public async Task<ActionResult<WeeklyRegistrationResponse>> GetWeekAsync(DateOnly weekStart, [FromQuery] Guid studentId, CancellationToken cancellationToken)
    {
        var result = await registrationService.GetWeekAsync(studentId, weekStart, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 以新資料覆蓋指定學生當週的搭乘登記設定。
    /// </summary>
    [HttpPut("weeks/{weekStart}")]
    public async Task<ActionResult<WeeklyRegistrationResponse>> UpdateWeekAsync(DateOnly weekStart, [FromBody] UpdateWeeklyRegistrationRequest request, CancellationToken cancellationToken)
    {
        var normalizedRequest = request with { WeekStart = weekStart };
        var result = await registrationService.UpdateWeekAsync(normalizedRequest, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 將前一週的登記模式複製到指定週次。
    /// </summary>
    [HttpPost("weeks/{weekStart}/copy-last-week")]
    public async Task<ActionResult<WeeklyRegistrationResponse>> CopyLastWeekAsync(DateOnly weekStart, [FromQuery] Guid studentId, CancellationToken cancellationToken)
    {
        var result = await registrationService.CopyLastWeekAsync(studentId, weekStart, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 取得指定學生的搭乘登記與實際出席摘要。
    /// </summary>
    [HttpGet("students/{studentId}/summary")]
    public async Task<ActionResult<StudentRegistrationSummaryResponse>> GetSummaryAsync(Guid studentId, CancellationToken cancellationToken)
    {
        var result = await registrationService.GetSummaryAsync(studentId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }
}
