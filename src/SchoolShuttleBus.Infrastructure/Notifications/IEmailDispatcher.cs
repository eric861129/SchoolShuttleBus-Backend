namespace SchoolShuttleBus.Infrastructure.Notifications;

internal interface IEmailDispatcher
{
    Task SendAsync(string recipientEmail, string subject, string body, CancellationToken cancellationToken);
}
