using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Contracts.Registrations;

namespace SchoolShuttleBus.Application.Registrations;

public interface IRegistrationService
{
    Task<Result<WeeklyRegistrationResponse>> GetWeekAsync(Guid studentId, DateOnly weekStart, CancellationToken cancellationToken);

    Task<Result<WeeklyRegistrationResponse>> UpdateWeekAsync(UpdateWeeklyRegistrationRequest request, CancellationToken cancellationToken);

    Task<Result<WeeklyRegistrationResponse>> CopyLastWeekAsync(Guid studentId, DateOnly weekStart, CancellationToken cancellationToken);

    Task<Result<StudentRegistrationSummaryResponse>> GetSummaryAsync(Guid studentId, CancellationToken cancellationToken);
}
