using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SchoolShuttleBus.Application.Common;
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
        payload.HasSubmittedWeek.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateWeek_ShouldAllowThursdayFirstSubmission_WhenNextWeekHasNotBeenSubmitted()
    {
        using var client = CreateClientAt(new DateTimeOffset(2026, 3, 19, 2, 0, 0, TimeSpan.Zero));
        var token = await client.LoginAndGetAccessTokenAsync("0900-000-003", "P@ssw0rd!");
        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/registrations/weeks/2026-03-23");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(new
        {
            StudentId = DemoSeedConstants.StudentId,
            WeekStart = new DateOnly(2026, 3, 23),
            Days = Enumerable.Range(0, 5).Select(offset => new
            {
                Date = new DateOnly(2026, 3, 23).AddDays(offset),
                ToSchool = true,
                Homebound = true,
                ToSchoolRouteId = DemoSeedConstants.MorningRouteId,
                HomeboundRouteId = DemoSeedConstants.DismissalRouteId
            }).ToArray()
        });

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<WeeklyRegistrationEnvelope>();
        payload.Should().NotBeNull();
        payload!.HasSubmittedWeek.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateWeek_ShouldRejectThursdayChanges_AfterNextWeekHasBeenSubmitted()
    {
        using var client = CreateClientAt(new DateTimeOffset(2026, 3, 19, 2, 0, 0, TimeSpan.Zero));
        var token = await client.LoginAndGetAccessTokenAsync("0900-000-003", "P@ssw0rd!");

        await SubmitNextWeekAsync(client, token);

        using var updateRequest = new HttpRequestMessage(HttpMethod.Put, "/api/registrations/weeks/2026-03-23");
        updateRequest.Headers.Authorization = new("Bearer", token);
        updateRequest.Content = JsonContent.Create(new
        {
            StudentId = DemoSeedConstants.StudentId,
            WeekStart = new DateOnly(2026, 3, 23),
            Days = Enumerable.Range(0, 5).Select(offset => new
            {
                Date = new DateOnly(2026, 3, 23).AddDays(offset),
                ToSchool = false,
                Homebound = false,
                ToSchoolRouteId = (Guid?)null,
                HomeboundRouteId = (Guid?)null
            }).ToArray()
        });

        var response = await client.SendAsync(updateRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var payload = await response.Content.ReadFromJsonAsync<ProblemEnvelope>();
        payload.Should().NotBeNull();
        payload!.Title.Should().Contain("Thursday");
    }

    [Fact]
    public async Task CopyLastWeek_ShouldRejectFridayRegistrations_WhenNextWeekIsClosed()
    {
        using var client = CreateClientAt(new DateTimeOffset(2026, 3, 20, 2, 0, 0, TimeSpan.Zero));
        var token = await client.LoginAndGetAccessTokenAsync("0900-000-003", "P@ssw0rd!");
        using var request = new HttpRequestMessage(HttpMethod.Post, $"/api/registrations/weeks/2026-03-23/copy-last-week?studentId={DemoSeedConstants.StudentId}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var payload = await response.Content.ReadFromJsonAsync<ProblemEnvelope>();
        payload.Should().NotBeNull();
        payload!.Title.Should().Contain("Friday");
    }

    private HttpClient CreateClientAt(DateTimeOffset utcNow) =>
        _factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<TimeProvider>();
                services.AddSingleton<TimeProvider>(new FixedTimeProvider(utcNow));
            });
        }).CreateClient();

    private static async Task SubmitNextWeekAsync(HttpClient client, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, "/api/registrations/weeks/2026-03-23");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(new
        {
            StudentId = DemoSeedConstants.StudentId,
            WeekStart = new DateOnly(2026, 3, 23),
            Days = Enumerable.Range(0, 5).Select(offset => new
            {
                Date = new DateOnly(2026, 3, 23).AddDays(offset),
                ToSchool = true,
                Homebound = true,
                ToSchoolRouteId = DemoSeedConstants.MorningRouteId,
                HomeboundRouteId = DemoSeedConstants.DismissalRouteId
            }).ToArray()
        });

        var response = await client.SendAsync(request);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private sealed class FixedTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => utcNow;
    }

    private sealed record RegistrationDayEnvelope(DateOnly Date, bool ToSchool, bool Homebound);

    private sealed record WeeklyRegistrationEnvelope(
        Guid StudentId,
        string StudentName,
        DateOnly WeekStart,
        IReadOnlyCollection<RegistrationDayEnvelope> Days,
        bool HasSubmittedWeek);

    private sealed record ProblemEnvelope(string Title);
}
