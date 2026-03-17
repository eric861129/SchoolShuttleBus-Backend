using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Contracts.Registrations;

public sealed record RegistrationDayRequest(DateOnly Date, bool ToSchool, bool Homebound, Guid? ToSchoolRouteId, Guid? HomeboundRouteId);

public sealed record UpdateWeeklyRegistrationRequest(Guid StudentId, DateOnly WeekStart, IReadOnlyCollection<RegistrationDayRequest> Days);

public sealed record RegistrationDayResponse(DateOnly Date, bool ToSchool, bool Homebound, Guid? ToSchoolRouteId, Guid? HomeboundRouteId);

public sealed record WeeklyRegistrationResponse(Guid StudentId, string StudentName, DateOnly WeekStart, IReadOnlyCollection<RegistrationDayResponse> Days);

public sealed record StudentRegistrationSummaryResponse(Guid StudentId, string StudentName, int RegisteredTrips, int PresentTrips, StudentStage Stage);
