using Microsoft.Extensions.Logging;

namespace SchoolShuttleBus.Infrastructure.Notifications;

internal sealed class ConsoleEmailDispatcher(ILogger<ConsoleEmailDispatcher> logger) : IEmailDispatcher
{
    public Task SendAsync(string recipientEmail, string subject, string body, CancellationToken cancellationToken)
    {
        logger.LogInformation("Demo email queued to {RecipientEmail}. Subject: {Subject}. Body: {Body}", recipientEmail, subject, body);
        return Task.CompletedTask;
    }
}
