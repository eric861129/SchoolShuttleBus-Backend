using System.Net.Http.Json;

namespace SchoolShuttleBus.Api.Tests;

internal static class TestAuthExtensions
{
    public static async Task<string> LoginAndGetAccessTokenAsync(this HttpClient client, string account, string password)
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new
        {
            account,
            password
        });

        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<TokenEnvelope>();
        return payload!.AccessToken;
    }

    private sealed record TokenEnvelope(string AccessToken, string RefreshToken);
}
