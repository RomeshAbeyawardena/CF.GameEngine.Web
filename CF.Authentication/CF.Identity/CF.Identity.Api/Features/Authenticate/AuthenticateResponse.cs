namespace CF.Identity.Api.Features.Authenticate;

public record AuthenticateResponse(bool IsSuccessful, string? Token, string? RefreshToken, DateTimeOffset? ExpiresAt, IEnumerable<string> Roles) : IAuthenticateResponse;
