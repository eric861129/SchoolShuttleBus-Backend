namespace SchoolShuttleBus.Domain.Registrations;

public static class RegistrationWindowPolicy
{
    public const int SubmittedTripSlotCount = 10;

    public static bool HasSubmittedWeek(int tripSlotCount) => tripSlotCount >= SubmittedTripSlotCount;

    public static bool CanSubmit(DateOnly currentDate, DateOnly targetWeekStart, bool hasSubmittedWeek)
    {
        if (!IsNextWeek(currentDate, targetWeekStart))
        {
            return true;
        }

        return currentDate.DayOfWeek switch
        {
            DayOfWeek.Thursday => !hasSubmittedWeek,
            DayOfWeek.Friday => false,
            _ => true
        };
    }

    public static bool ShouldShowReminder(DateOnly currentDate, DateOnly targetWeekStart, bool hasSubmittedWeek)
    {
        if (hasSubmittedWeek || !IsNextWeek(currentDate, targetWeekStart))
        {
            return false;
        }

        return currentDate.DayOfWeek is DayOfWeek.Wednesday or DayOfWeek.Thursday;
    }

    private static bool IsNextWeek(DateOnly currentDate, DateOnly targetWeekStart) => targetWeekStart == GetNextWeekStart(currentDate);

    private static DateOnly GetNextWeekStart(DateOnly currentDate)
    {
        var diff = ((int)DayOfWeek.Monday - (int)currentDate.DayOfWeek + 7) % 7;
        if (diff == 0)
        {
            diff = 7;
        }

        return currentDate.AddDays(diff);
    }
}
