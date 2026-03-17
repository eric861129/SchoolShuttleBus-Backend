using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Contracts.Attendance;

public sealed record CreateAttendanceSessionRequest(Guid RouteId, DateOnly Date, TripDirection Direction);

public sealed record UpdateAttendanceRecordRequest(AttendanceStatus Status);

public sealed record AttendanceRecordResponse(Guid AttendanceRecordId, Guid StudentId, string StudentName, AttendanceStatus Status, string EmergencyPhoneSnapshot);

public sealed record AttendanceSessionResponse(
    Guid AttendanceSessionId,
    Guid RouteId,
    string RouteName,
    DateOnly Date,
    TripDirection Direction,
    bool IsCompleted,
    IReadOnlyCollection<AttendanceRecordResponse> Records);
