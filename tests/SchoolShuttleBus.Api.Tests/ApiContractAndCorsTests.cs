using System.Net;
using System.Text.Json;
using FluentAssertions;

namespace SchoolShuttleBus.Api.Tests;

public sealed class ApiContractAndCorsTests : IClassFixture<SchoolShuttleBusApiFactory>
{
    private readonly SchoolShuttleBusApiFactory _factory;

    public ApiContractAndCorsTests(SchoolShuttleBusApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Swagger_ShouldExposeAuthContextAndAdminLookups_AndReminderRunResponseSchema()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/swagger/v1/swagger.json");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;

        root.GetProperty("paths").TryGetProperty("/api/Auth/context", out _).Should().BeTrue();
        root.GetProperty("paths").TryGetProperty("/api/Admin/lookups", out _).Should().BeTrue();

        var reminderRun = root.GetProperty("paths")
            .GetProperty("/api/Notifications/reminders/run")
            .GetProperty("post")
            .GetProperty("responses")
            .GetProperty("200")
            .GetProperty("content")
            .GetProperty("application/json")
            .GetProperty("schema")
            .GetProperty("$ref")
            .GetString();

        reminderRun.Should().Be("#/components/schemas/ReminderRunResponse");
    }

    [Fact]
    public async Task Cors_Preflight_AllowedOrigin_ReturnsAccessControlAllowOrigin()
    {
        using var client = _factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Options, "/api/auth/me");
        request.Headers.Add("Origin", "https://frontend.test");
        request.Headers.Add("Access-Control-Request-Method", "GET");

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        response.Headers.TryGetValues("Access-Control-Allow-Origin", out var values).Should().BeTrue();
        values.Should().ContainSingle("https://frontend.test");
    }

    [Fact]
    public async Task Cors_Request_DisallowedOrigin_DoesNotReturnCorsHeaders()
    {
        using var client = _factory.CreateClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/health");
        request.Headers.Add("Origin", "https://disallowed.test");

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Headers.Contains("Access-Control-Allow-Origin").Should().BeFalse();
    }
}
