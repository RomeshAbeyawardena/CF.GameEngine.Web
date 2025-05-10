namespace CF.Identity.Api.Features.TokenExchange;

public record TokenResponse(
    string AccessToken,
    string TokenType,
    string ExpiresIn,
    string RefreshToken,
    string Scope
);
