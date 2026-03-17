using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SchoolShuttleBus.Domain.Shared;
using SchoolShuttleBus.Infrastructure.Persistence;

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

    [Theory]
    [InlineData("S10001", "陳小明", true, false)]
    [InlineData("0900-000-003", "陳家長", true, false)]
    [InlineData("T0001", "王老師", false, true)]
    [InlineData("E0001", "系統管理員", false, true)]
    public async Task Context_ShouldReturnFrontendBootstrapData_ForSupportedRoles(
        string account,
        string displayName,
        bool expectStudents,
        bool expectStaffProfile)
    {
        using var client = _factory.CreateClient();
        var token = await client.LoginAndGetAccessTokenAsync(account, "P@ssw0rd!");
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/context");
        request.Headers.Authorization = new("Bearer", token);

        var response = await client.SendAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<CurrentUserContextEnvelope>();
        payload.Should().NotBeNull();
        payload!.DisplayName.Should().Be(displayName);
        payload.Students.Should().HaveCount(expectStudents ? 1 : 0);
        (payload.StaffProfile is not null).Should().Be(expectStaffProfile);

        if (expectStudents)
        {
            payload.Students[0].StudentId.Should().Be(DemoSeedConstants.StudentId);
            payload.Students[0].StudentNumber.Should().Be("S10001");
            payload.Students[0].StudentName.Should().Be("陳小明");
            payload.Students[0].Stage.Should().Be(StudentStage.JuniorHigh);
            payload.Students[0].GradeLabel.Should().Be("八年級");
        }

        if (expectStaffProfile)
        {
            payload.StaffProfile.Should().NotBeNull();
            payload.StaffProfile!.FullName.Should().Be(displayName);
        }
    }

    private sealed record TokenEnvelope(string AccessToken, string RefreshToken);

    private sealed record CurrentUserEnvelope(Guid UserId, string Email, IReadOnlyCollection<string> Roles);

    private sealed record CurrentUserContextEnvelope(
        Guid UserId,
        string Email,
        IReadOnlyCollection<string> Roles,
        string DisplayName,
        IReadOnlyList<AccessibleStudentEnvelope> Students,
        StaffProfileSummaryEnvelope? StaffProfile);

    private sealed record AccessibleStudentEnvelope(
        Guid StudentId,
        string StudentNumber,
        string StudentName,
        StudentStage Stage,
        string GradeLabel);

    private sealed record StaffProfileSummaryEnvelope(
        Guid StaffProfileId,
        string EmployeeNumber,
        string FullName,
        bool CanManageAllRoutes);
}
