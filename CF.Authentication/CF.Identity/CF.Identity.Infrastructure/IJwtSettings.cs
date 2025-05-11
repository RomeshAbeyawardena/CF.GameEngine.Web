namespace CF.Identity.Infrastructure;

public interface IJwtSettings
{
    string? Issuer { get; }
    string? Audience { get; }
    string? SigningKey { get; }
}
