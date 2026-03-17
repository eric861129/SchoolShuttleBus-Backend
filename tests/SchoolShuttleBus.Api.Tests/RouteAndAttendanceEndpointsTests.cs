using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SchoolShuttleBus.Domain.Shared;
using SchoolShuttleBus.Infrastructure.Persistence;

namespace SchoolShuttleBus.Api.Tests;

public sealed class RouteAndAttendanceEndpointsTests : IClassFixture<SchoolShuttleBusApiFactory>
{
    private readonly SchoolShuttleBusApiFactory _factory;

    public RouteAndAttendanceEndpointsTests(SchoolShuttleBusApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetRoutes_ShouldReturnAssignedRoute_ForTeacher()
    {
        using var client = _factory.CreateClient();
        var token = await client.LoginAndGetAccessTokenAsync("T0001", "P@ssw0rd!");
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/routes");
        request.Headers.Authorization = new("Bearer", token);

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var routes = await response.Content.ReadFromJsonAsync<IReadOnlyCollection<RouteEnvelope>>();
        routes.Should().NotBeNull();
        routes!.Select(static route => route.RouteId).Should().Contain(DemoSeedConstants.MorningRouteId);
    }

    [Fact]
    public async Task CreateAttendanceSession_ShouldCreateManifestForRouteRegistrations()
    {
        using var client = _factory.CreateClient();
        var token = await client.LoginAndGetAccessTokenAsync("T0001", "P@ssw0rd!");
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/attendance/sessions");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(new
        {
            routeId = DemoSeedConstants.MorningRouteId,
            date = "2026-03-16",
            direction = TripDirection.ToSchool
        });

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var session = await response.Content.ReadFromJsonAsync<AttendanceSessionEnvelope>();
        session.Should().NotBeNull();
        session!.Records.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateAttendanceSession_ShouldRejectMismatchedRouteDirection()
    {
        using var client = _factory.CreateClient();
        var token = await client.LoginAndGetAccessTokenAsync("T0001", "P@ssw0rd!");
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/attendance/sessions");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(new
        {
            routeId = DemoSeedConstants.MorningRouteId,
            date = "2026-03-16",
            direction = TripDirection.Homebound
        });

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateAttendanceSession_ShouldRejectWhenNoRegistrationsExist()
    {
        using var client = _factory.CreateClient();
        var token = await client.LoginAndGetAccessTokenAsync("T0001", "P@ssw0rd!");
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/attendance/sessions");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(new
        {
            routeId = DemoSeedConstants.MorningRouteId,
            date = "2026-03-30",
            direction = TripDirection.ToSchool
        });

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private sealed record RouteEnvelope(Guid RouteId, string RouteName);

    private sealed record AttendanceRecordEnvelope(Guid AttendanceRecordId, Guid StudentId, string StudentName, AttendanceStatus Status, string EmergencyPhoneSnapshot);

    private sealed record AttendanceSessionEnvelope(Guid AttendanceSessionId, Guid RouteId, string RouteName, DateOnly Date, TripDirection Direction, bool IsCompleted, IReadOnlyCollection<AttendanceRecordEnvelope> Records);
}
