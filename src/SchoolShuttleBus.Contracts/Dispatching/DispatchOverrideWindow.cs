using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Contracts.Dispatching;

public sealed record DispatchOverrideWindow(
    Guid StudentId,
    DateOnly Date,
    TripDirection Direction,
    Guid RouteId);
