using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SchoolShuttleBus.Infrastructure.Persistence;

namespace SchoolShuttleBus.Api.Tests;

public sealed class RegistrationEndpointsTests : IClassFixture<SchoolShuttleBusApiFactory>
{
    private readonly SchoolShuttleBusApiFactory _factory;

    public RegistrationEndpointsTests(SchoolShuttleBusApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetWeek_ShouldReturnFiveDays_ForLinkedParent()
    {
        using var client = _factory.CreateClient();
        var token = await client.LoginAndGetAccessTokenAsync("0900-000-003", "P@ssw0rd!");
        using var request = new HttpRequestMessage(HttpMethod.Get, $"/api/registrations/weeks/2026-03-16?studentId={DemoSeedConstants.StudentId}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<WeeklyRegistrationEnvelope>();
        payload.Should().NotBeNull();
        payload!.Days.Should().HaveCount(5);
    }

    [Fact]
    public async Task CopyLastWeek_ShouldDuplicatePreviousWeekSchedule()
    {
        using var client = _factory.CreateClient();
        var token = await client.LoginAndGetAccessTokenAsync("0900-000-003", "P@ssw0rd!");
        using var request = new HttpRequestMessage(HttpMethod.Post, $"/api/registrations/weeks/2026-03-23/copy-last-week?studentId={DemoSeedConstants.StudentId}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<WeeklyRegistrationEnvelope>();
        payload.Should().NotBeNull();
        payload!.Days.Count(static day => day.ToSchool).Should().Be(5);
        payload.Days.Count(static day => day.Homebound).Should().Be(5);
    }

    private sealed record RegistrationDayEnvelope(DateOnly Date, bool ToSchool, bool Homebound);

    private sealed record WeeklyRegistrationEnvelope(Guid StudentId, string StudentName, DateOnly WeekStart, IReadOnlyCollection<RegistrationDayEnvelope> Days);
}
