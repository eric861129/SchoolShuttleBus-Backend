using SchoolShuttleBus.Contracts.Dispatching;

namespace SchoolShuttleBus.Application.Dispatching;

/// <summary>
/// Detects whether an administrator is trying to add a duplicate route override
/// for the same student, date, and trip direction.
/// </summary>
public sealed class DispatchConflictDetector
{
    public DispatchConflictResult Detect(IEnumerable<DispatchOverrideWindow> existingOverrides, DispatchOverrideWindow candidate)
    {
        var hasConflict = existingOverrides.Any(existing =>
            existing.StudentId == candidate.StudentId &&
            existing.Date == candidate.Date &&
            existing.Direction == candidate.Direction);

        return hasConflict
            ? new DispatchConflictResult(true, "A dispatch override already exists for the same student, date, and direction.")
            : new DispatchConflictResult(false, null);
    }
}
