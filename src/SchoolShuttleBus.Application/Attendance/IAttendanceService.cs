using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Contracts.Attendance;

namespace SchoolShuttleBus.Application.Attendance;

public interface IAttendanceService
{
    Task<IReadOnlyCollection<AttendanceSessionResponse>> GetSessionsAsync(CancellationToken cancellationToken);

    Task<Result<AttendanceSessionResponse>> CreateSessionAsync(CreateAttendanceSessionRequest request, CancellationToken cancellationToken);

    Task<Result<AttendanceRecordResponse>> UpdateRecordAsync(Guid recordId, UpdateAttendanceRecordRequest request, CancellationToken cancellationToken);

    Task<Result<AttendanceSessionResponse>> CompleteSessionAsync(Guid sessionId, CancellationToken cancellationToken);
}
