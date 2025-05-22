namespace CF.Identity.Api.Features.TokenExchange;

public record TokenRequest(
    string GrantType,
    string ClientId,
    string ClientSecret,
    string Scope,
    string Username
)
{
    public string? RedirectUri { get; init; }
}
