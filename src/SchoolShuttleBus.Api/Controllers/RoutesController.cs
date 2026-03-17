using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolShuttleBus.Application.Routes;
using SchoolShuttleBus.Contracts.Routes;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Api.Controllers;

/// <summary>
/// Route, stop, and assignment management endpoints.
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
public sealed class RoutesController(IRouteService routeService) : ControllerBase
{
    /// <summary>
    /// Returns routes visible to the current user.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<RouteResponse>>> GetRoutesAsync(CancellationToken cancellationToken)
        => Ok(await routeService.GetRoutesAsync(cancellationToken));

    /// <summary>
    /// Creates a new route definition.
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost]
    public async Task<ActionResult<RouteResponse>> CreateRouteAsync([FromBody] CreateRouteRequest request, CancellationToken cancellationToken)
    {
        var result = await routeService.CreateRouteAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// Updates route metadata without replacing stops or assignments.
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPatch("{routeId:guid}")]
    public async Task<ActionResult<RouteResponse>> UpdateRouteAsync(Guid routeId, [FromBody] UpdateRouteRequest request, CancellationToken cancellationToken)
    {
        var result = await routeService.UpdateRouteAsync(routeId, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// Replaces the stop list for a route.
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost("{routeId:guid}/stops")]
    public async Task<ActionResult<RouteResponse>> UpsertStopsAsync(Guid routeId, [FromBody] UpsertRouteStopsRequest request, CancellationToken cancellationToken)
    {
        var result = await routeService.UpsertStopsAsync(routeId, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }

    /// <summary>
    /// Assigns a teacher profile to a route.
    /// </summary>
    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost("assignments/{routeId:guid}")]
    public async Task<ActionResult<RouteResponse>> AssignStaffAsync(Guid routeId, [FromBody] UpsertRouteAssignmentRequest request, CancellationToken cancellationToken)
    {
        var result = await routeService.AssignStaffAsync(routeId, request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new ProblemDetails { Title = result.Error });
    }
}
