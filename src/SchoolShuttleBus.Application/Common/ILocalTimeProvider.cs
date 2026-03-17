namespace SchoolShuttleBus.Application.Common;

public interface ILocalTimeProvider
{
    DateOnly Today { get; }

    DateTimeOffset Now { get; }

    DateOnly NextWeekStart { get; }
}
