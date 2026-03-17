using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Contracts.Routes;

public sealed record CreateRouteRequest(string RouteName, RouteType RouteType, TripDirection Direction, string CampusName);

public sealed record UpdateRouteRequest(string RouteName, RouteType RouteType, bool IsActive);

public sealed record RouteStopRequest(int Sequence, string StopName, string Address, string? HandoffContactName, string? HandoffContactPhone);

public sealed record UpsertRouteStopsRequest(IReadOnlyCollection<RouteStopRequest> Stops);

public sealed record UpsertRouteAssignmentRequest(Guid StaffProfileId);

public sealed record RouteStopResponse(Guid RouteStopId, int Sequence, string StopName, string Address, string? HandoffContactName, string? HandoffContactPhone);

public sealed record RouteAssignmentResponse(Guid RouteAssignmentId, Guid StaffProfileId, string StaffName);

public sealed record RouteResponse(
    Guid RouteId,
    string RouteName,
    RouteType RouteType,
    TripDirection Direction,
    string CampusName,
    bool IsActive,
    IReadOnlyCollection<RouteStopResponse> Stops,
    IReadOnlyCollection<RouteAssignmentResponse> Assignments);
