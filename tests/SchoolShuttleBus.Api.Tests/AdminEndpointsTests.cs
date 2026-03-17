using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Api.Tests;

public sealed class AdminEndpointsTests : IClassFixture<SchoolShuttleBusApiFactory>
{
    private readonly SchoolShuttleBusApiFactory _factory;

    public AdminEndpointsTests(SchoolShuttleBusApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateReport_ShouldGenerateDownloadableCsv()
    {
        using var client = _factory.CreateClient();
        var token = await client.LoginAndGetAccessTokenAsync("admin@demo.local", "P@ssw0rd!");
        using var createRequest = new HttpRequestMessage(HttpMethod.Post, "/api/admin/reports");
        createRequest.Headers.Authorization = new("Bearer", token);
        createRequest.Content = JsonContent.Create(new
        {
            reportType = ReportType.WeeklyRegistrations,
            exportFormat = ExportFormat.Csv,
            startDate = "2026-03-16",
            endDate = "2026-03-20"
        });

        var createResponse = await client.SendAsync(createRequest);

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var report = await createResponse.Content.ReadFromJsonAsync<ReportEnvelope>();
        report.Should().NotBeNull();

        using var downloadRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/admin/reports/{report!.ReportExportId}");
        downloadRequest.Headers.Authorization = new("Bearer", token);
        var downloadResponse = await client.SendAsync(downloadRequest);

        downloadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        (await downloadResponse.Content.ReadAsStringAsync()).Should().Contain("StudentName");
    }

    [Fact]
    public async Task RunReminders_ShouldCreateNotificationJob()
    {
        using var client = _factory.CreateClient();
        var token = await client.LoginAndGetAccessTokenAsync("admin@demo.local", "P@ssw0rd!");
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/notifications/reminders/run");
        request.Headers.Authorization = new("Bearer", token);

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<ReminderEnvelope>();
        payload.Should().NotBeNull();
        payload!.DeliveryCount.Should().BeGreaterThan(0);
    }

    private sealed record ReportEnvelope(Guid ReportExportId, string FileName, string ContentType, ReportType ReportType, ExportFormat ExportFormat, DateTimeOffset CreatedAtUtc);

    private sealed record ReminderEnvelope(Guid NotificationJobId, int DeliveryCount);
}
