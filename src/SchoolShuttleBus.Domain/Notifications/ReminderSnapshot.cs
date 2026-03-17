using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Notifications;

public sealed class ReminderSnapshot
{
    private ReminderSnapshot(UserRole role, DateOnly currentDate, bool hasCompletedNextWeekRegistration)
    {
        Role = role;
        CurrentDate = currentDate;
        HasCompletedNextWeekRegistration = hasCompletedNextWeekRegistration;
    }

    public UserRole Role { get; }

    public DateOnly CurrentDate { get; }

    public bool HasCompletedNextWeekRegistration { get; }

    public static ReminderSnapshot Create(UserRole role, DateOnly currentDate, bool hasCompletedNextWeekRegistration)
        => new(role, currentDate, hasCompletedNextWeekRegistration);
}
