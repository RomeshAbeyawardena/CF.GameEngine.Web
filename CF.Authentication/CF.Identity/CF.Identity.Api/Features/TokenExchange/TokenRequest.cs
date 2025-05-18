namespace CF.Identity.Api.Features.TokenExchange;

public record TokenRequest(
    string GrantType,
    string ClientId,
    string ClientSecret,
    string Scope,
    string RedirectUri,
    string Username
);
