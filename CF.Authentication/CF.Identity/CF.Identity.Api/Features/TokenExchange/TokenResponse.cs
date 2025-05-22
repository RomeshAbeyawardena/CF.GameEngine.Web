namespace CF.Identity.Api.Features.TokenExchange;

public record TokenResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    string RefreshToken,
    string Scope
);
