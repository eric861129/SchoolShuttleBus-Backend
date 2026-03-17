using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Application.Notifications;

namespace SchoolShuttleBus.Infrastructure.Notifications;

internal sealed class ReminderBackgroundService(
    IServiceScopeFactory serviceScopeFactory,
    ILocalTimeProvider localTimeProvider,
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
                var localNow = localTimeProvider.Now;
                var localDate = localTimeProvider.Today;

                if (localNow.Hour == _options.RunHourLocal &&
                    localNow.DayOfWeek is DayOfWeek.Wednesday or DayOfWeek.Thursday &&
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
