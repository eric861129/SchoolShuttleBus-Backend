namespace SchoolShuttleBus.Application.Dispatching;

public sealed record DispatchConflictResult(bool HasConflict, string? Reason);
