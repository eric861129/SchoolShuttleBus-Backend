namespace SchoolShuttleBus.Domain.Notifications;

public static class ReminderPolicy
{
    public static bool ShouldSendReminder(ReminderSnapshot snapshot)
    {
        if (snapshot.HasCompletedNextWeekRegistration)
        {
            return false;
        }

        return snapshot.CurrentDate.DayOfWeek is DayOfWeek.Thursday or DayOfWeek.Friday;
    }
}
