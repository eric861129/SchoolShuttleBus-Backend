namespace SchoolShuttleBus.Infrastructure.Notifications;

internal sealed class MailjetOptions
{
    public const string SectionName = "Mailjet";

    public bool Enabled { get; set; }

    public string ApiKey { get; set; } = string.Empty;

    public string ApiSecret { get; set; } = string.Empty;

    public string FromEmail { get; set; } = "no-reply@demo.local";

    public string FromName { get; set; } = "School Shuttle Bus Demo";
}
