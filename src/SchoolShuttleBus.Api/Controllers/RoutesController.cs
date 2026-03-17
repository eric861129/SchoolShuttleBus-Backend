using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolShuttleBus.Application.Routes;
using SchoolShuttleBus.Contracts.Routes;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Api.Controllers;

/// <summary>
/// 提供路線、停靠站與教師指派管理的相關端點。
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class RoutesController(IRouteService routeService) : ControllerBase
{
    /// <summary>
    /// 取得目前使用者可查看的路線資料。
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<RouteResponse>>> GetRoutesAsync(CancellationToken cancellationToken)
        => Ok(await routeService.GetRoutesAsync(cancellationToken));

    /// <summary>
    /// 建立一筆新的路線定義。
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost]
    public async Task<ActionResult<RouteResponse>> CreateRouteAsync([FromBody] CreateRouteRequest request, CancellationToken cancellationToken)
    {
        var result = await routeService.CreateRouteAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 更新路線基本資料，不直接覆蓋停靠站與指派資訊。
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPatch("{routeId:guid}")]
    public async Task<ActionResult<RouteResponse>> UpdateRouteAsync(Guid routeId, [FromBody] UpdateRouteRequest request, CancellationToken cancellationToken)
    {
        var result = await routeService.UpdateRouteAsync(routeId, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 以新清單覆蓋指定路線的停靠站資料。
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost("{routeId:guid}/stops")]
    public async Task<ActionResult<RouteResponse>> UpsertStopsAsync(Guid routeId, [FromBody] UpsertRouteStopsRequest request, CancellationToken cancellationToken)
    {
        var result = await routeService.UpsertStopsAsync(routeId, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// 將指定教師指派到特定路線。
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost("assignments/{routeId:guid}")]
    public async Task<ActionResult<RouteResponse>> AssignStaffAsync(Guid routeId, [FromBody] UpsertRouteAssignmentRequest request, CancellationToken cancellationToken)
    {
        var result = await routeService.AssignStaffAsync(routeId, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }
}
