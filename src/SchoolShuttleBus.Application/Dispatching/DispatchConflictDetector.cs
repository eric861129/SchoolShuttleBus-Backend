using SchoolShuttleBus.Contracts.Dispatching;

namespace SchoolShuttleBus.Application.Dispatching;

/// <summary>
/// 判斷管理者是否針對同一位學生、同一天與同一搭乘方向
/// 重複建立了衝突的路線覆寫設定。
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
