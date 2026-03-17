using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SchoolShuttleBus.Application.Notifications;

namespace SchoolShuttleBus.Infrastructure.Notifications;

internal sealed class ReminderBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    IOptions<ReminderOptions> options,
    ILogger<ReminderBackgroundService> logger) : BackgroundService
{
    private readonly ReminderOptions _options = options.Value;
    private DateOnly? _lastRunDate;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.Enabled)
        {
            logger.LogInformation("Reminder background service is disabled.");
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(_options.TimeZoneId);
                var localNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);
                var localDate = DateOnly.FromDateTime(localNow.DateTime);

                if (localNow.Hour == _options.RunHourLocal &&
                    localNow.DayOfWeek is DayOfWeek.Thursday or DayOfWeek.Friday &&
                    _lastRunDate != localDate)
                {
                    await using var scope = serviceScopeFactory.CreateAsyncScope();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                    await notificationService.RunRemindersAsync(actorUserId: null, stoppingToken);
                    _lastRunDate = localDate;
                }
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Reminder background service failed during execution.");
            }

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
