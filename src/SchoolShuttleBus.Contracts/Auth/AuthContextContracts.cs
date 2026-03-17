using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Contracts.Auth;

public sealed record CurrentUserContextResponse(
    Guid UserId,
    string Email,
    IReadOnlyCollection<string> Roles,
    string DisplayName,
    IReadOnlyCollection<AccessibleStudentResponse> Students,
    StaffProfileSummaryResponse? StaffProfile);

public sealed record AccessibleStudentResponse(
    Guid StudentId,
    string StudentNumber,
    string StudentName,
    StudentStage Stage,
    string GradeLabel);

public sealed record StaffProfileSummaryResponse(
    Guid StaffProfileId,
    string EmployeeNumber,
    string FullName,
    bool CanManageAllRoutes);
