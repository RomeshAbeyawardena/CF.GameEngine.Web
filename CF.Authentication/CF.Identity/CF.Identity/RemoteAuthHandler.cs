using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CF.Identity;

public class RemoteAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IHttpClientFactory httpClientFactory) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("RemoteAuth");

    private static void AddClaims(ClaimsIdentity identity, AuthenticationResponse body)
    {
        if (!string.IsNullOrWhiteSpace(body.Token))
        {
            identity.AddClaim(new Claim("Token", body.Token));
        }

        if (!string.IsNullOrWhiteSpace(body.RefreshToken))
        {
            identity.AddClaim(new Claim("RefreshToken", body.RefreshToken));
        }

        if (body.ExpiresAt.HasValue)
        {
            identity.AddClaim(new Claim("Expires", body.ExpiresAt.Value.ToUnixTimeMilliseconds().ToString()));
        }

        if (body.Roles.Any())
        {
            foreach (var role in body.Roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }
    }

    private async Task<AuthenticateResult> FreshClientRequest()
    {
        if (!Request.Headers.TryGetValue("X-ApiKey", out var apiKey) ||
            !Request.Headers.TryGetValue("X-Username", out var username) ||
            !Request.Headers.TryGetValue("X-Secret", out var secret))
        {
            return AuthenticateResult.Fail("Missing credentials");
        }

        var payload = new
        {
            ApiKey = apiKey.ToString(),
            UserName = username.ToString(),
            Secret = secret.ToString()
        };

        var response = await _httpClient.PostAsJsonAsync("auth/authenticate", payload);
        var body = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
        if (!response.IsSuccessStatusCode || body is null || string.IsNullOrWhiteSpace(body.Token))
        {
            return AuthenticateResult.Fail("Invalid credentials or malformed response");
        }

        var identity = new ClaimsIdentity(Scheme.Name);
        if (!string.IsNullOrWhiteSpace(username))
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, username.ToString()));
        }
        identity.AddClaim(new Claim("ApiKey", apiKey.ToString()));
        AddClaims(identity, body);

        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private async Task<AuthenticateResult> PreAuthenticatedClientRequest(StringValues authToken)
    {
        var payload = new
        {
            AuthToken = authToken.ToString()
        };

        var response = await _httpClient.PostAsJsonAsync("auth/authenticate/refresh", payload);
        var body = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

        var identity = new ClaimsIdentity(Scheme.Name);

        if (!response.IsSuccessStatusCode || body is null || string.IsNullOrWhiteSpace(body.Token))
        {
            return AuthenticateResult.Fail("Invalid credentials or malformed response");
        }

        AddClaims(identity, body);

        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (Request.Headers.TryGetValue("x-auth-token", out var authToken))
        {
            return await PreAuthenticatedClientRequest(authToken);
        }

        return await FreshClientRequest();
    }
}

