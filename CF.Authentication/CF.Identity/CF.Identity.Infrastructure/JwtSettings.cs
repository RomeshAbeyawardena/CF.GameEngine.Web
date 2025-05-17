using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CF.Identity.Infrastructure;

public record JwtSettings(string? Issuer, string? Audience, string? SigningKey) : IJwtSettings;

public record ConfigurationDerivedJwtSettings : IJwtSettings
{
    public ConfigurationDerivedJwtSettings(ILogger<IJwtSettings> logger, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        logger.LogInformation(jwtSettings.Value);
        jwtSettings.Bind(this);
    }

    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? SigningKey { get; set; }
}
