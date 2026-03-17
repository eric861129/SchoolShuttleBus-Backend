using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SchoolShuttleBus.Infrastructure.Notifications;

internal sealed class MailjetEmailDispatcher(
    HttpClient httpClient,
    IOptions<MailjetOptions> options,
    ILogger<MailjetEmailDispatcher> logger) : IEmailDispatcher
{
    private readonly MailjetOptions _options = options.Value;

    public async Task SendAsync(string recipientEmail, string subject, string body, CancellationToken cancellationToken)
    {
        if (!_options.Enabled || string.IsNullOrWhiteSpace(_options.ApiKey) || string.IsNullOrWhiteSpace(_options.ApiSecret))
        {
            logger.LogInformation("Mailjet is disabled or unconfigured. Simulating email to {RecipientEmail}. Subject: {Subject}", recipientEmail, subject);
            return;
        }

        var payload = JsonSerializer.Serialize(new
        {
            Messages = new[]
            {
                new
                {
                    From = new { Email = _options.FromEmail, Name = _options.FromName },
                    To = new[] { new { Email = recipientEmail } },
                    Subject = subject,
                    TextPart = body
                }
            }
        });

        using var request = new HttpRequestMessage(HttpMethod.Post, "v3.1/send");
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_options.ApiKey}:{_options.ApiSecret}")));
        request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

        using var response = await httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            logger.LogWarning("Mailjet request failed for {RecipientEmail}: {StatusCode} {Error}", recipientEmail, response.StatusCode, error);
        }
    }
}
