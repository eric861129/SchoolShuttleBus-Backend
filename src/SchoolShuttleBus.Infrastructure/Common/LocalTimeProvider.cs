using Microsoft.Extensions.Options;
using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Infrastructure.Notifications;

namespace SchoolShuttleBus.Infrastructure.Common;

internal sealed class LocalTimeProvider(
    TimeProvider timeProvider,
    IOptions<ReminderOptions> reminderOptions) : ILocalTimeProvider
{
    private readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById(reminderOptions.Value.TimeZoneId);

    public DateTimeOffset Now => TimeZoneInfo.ConvertTime(timeProvider.GetUtcNow(), _timeZone);

    public DateOnly Today => DateOnly.FromDateTime(Now.DateTime);

    public DateOnly NextWeekStart => GetNextWeekStart(Today);

    private static DateOnly GetNextWeekStart(DateOnly today)
    {
        var diff = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
        if (diff == 0)
        {
            diff = 7;
        }

        return today.AddDays(diff);
    }
}
