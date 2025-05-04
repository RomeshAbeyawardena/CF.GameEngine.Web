namespace CF.Identity;

public interface IAuthenticateResponse
{
    string? Token { get; }
    string? RefreshToken { get; }
    DateTimeOffset? ExpiresAt { get; }
    IEnumerable<string> Roles { get; }
}

public record AuthenticationResponse(string? Token, string? RefreshToken, DateTimeOffset? ExpiresAt, IEnumerable<string> Roles) : IAuthenticateResponse
{
    public AuthenticationResponse(IAuthenticateResponse response) : this(response.Token, response.RefreshToken, response.ExpiresAt, response.Roles)
    {
    }
}
