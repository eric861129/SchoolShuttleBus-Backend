using System.Net;
using System.Net.Http.Json;
using FluentAssertions;

namespace SchoolShuttleBus.Api.Tests;

public sealed class AuthEndpointsTests : IClassFixture<SchoolShuttleBusApiFactory>
{
    private readonly SchoolShuttleBusApiFactory _factory;

    public AuthEndpointsTests(SchoolShuttleBusApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_ShouldReturnAccessAndRefreshToken_ForSeededAdmin()
    {
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            account = "E0001",
            password = "P@ssw0rd!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<TokenEnvelope>();
        payload.Should().NotBeNull();
        payload!.AccessToken.Should().NotBeNullOrWhiteSpace();
        payload.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Me_ShouldReturnCurrentUserProfile_WhenBearerTokenIsValid()
    {
        using var client = _factory.CreateClient();
        var login = await client.PostAsJsonAsync("/api/auth/login", new
        {
            account = "E0001",
            password = "P@ssw0rd!"
        });

        var token = await login.Content.ReadFromJsonAsync<TokenEnvelope>();
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/me");
        request.Headers.Authorization = new("Bearer", token!.AccessToken);

        var response = await client.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        var authHeader = string.Join(" | ", response.Headers.WwwAuthenticate.Select(static value => value.ToString()));

        response.StatusCode.Should().Be(HttpStatusCode.OK, $"body: {content}; auth: {authHeader}");

        var me = System.Text.Json.JsonSerializer.Deserialize<CurrentUserEnvelope>(content, new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        me.Should().NotBeNull();
        me!.Email.Should().Be("admin@demo.local");
        me.Roles.Should().Contain("Administrator");
    }

    [Theory]
    [InlineData("E0001")]
    [InlineData("S10001")]
    [InlineData("0900-000-003")]
    public async Task Login_ShouldAcceptConfiguredAccountIdentifiers(string account)
    {
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            account,
            password = "P@ssw0rd!"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<TokenEnvelope>();
        payload.Should().NotBeNull();
        payload!.AccessToken.Should().NotBeNullOrWhiteSpace();
    }

    private sealed record TokenEnvelope(string AccessToken, string RefreshToken);

    private sealed record CurrentUserEnvelope(Guid UserId, string Email, IReadOnlyCollection<string> Roles);
}
