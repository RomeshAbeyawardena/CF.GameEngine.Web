using Microsoft.Extensions.Configuration;

namespace CF.Identity.Infrastructure;

public record JwtSettings(string? Issuer, string? Audience, string? SigningKey) : IJwtSettings;

public record ConfigurationDerivedJwtSettings : IJwtSettings
{
    public ConfigurationDerivedJwtSettings(IConfiguration configuration)
    {
        configuration.GetRequiredSection("JwtSettings")
            .Bind(this);
    }

    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? SigningKey { get; set; }
}
