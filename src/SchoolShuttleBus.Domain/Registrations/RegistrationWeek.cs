namespace SchoolShuttleBus.Domain.Registrations;

public sealed class RegistrationWeek
{
    private RegistrationWeek(DateOnly weekStart, IReadOnlyList<RegistrationDay> days)
    {
        WeekStart = weekStart;
        Days = days;
    }

    public DateOnly WeekStart { get; }

    public IReadOnlyList<RegistrationDay> Days { get; }

    public static RegistrationWeek Create(DateOnly weekStart)
    {
        if (weekStart.DayOfWeek != DayOfWeek.Monday)
        {
            throw new ArgumentException("The registration week must start on Monday.", nameof(weekStart));
        }

        var days = Enumerable.Range(0, 5)
            .Select(offset => new RegistrationDay(weekStart.AddDays(offset)))
            .ToArray();

        return new RegistrationWeek(weekStart, days);
    }
}
