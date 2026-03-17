namespace SchoolShuttleBus.Infrastructure.Notifications;

internal sealed class ReminderOptions
{
    public const string SectionName = "Reminders";

    public bool Enabled { get; set; } = true;

    public string TimeZoneId { get; set; } = "Asia/Taipei";

    public int RunHourLocal { get; set; } = 9;
}
