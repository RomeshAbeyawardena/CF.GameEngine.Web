namespace CF.Identity.Api.Features.TokenExchange;

public record TokenRequest(
    string GrantType,
    string Scope,
    string Username)
{
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? RedirectUri { get; init; }
}
