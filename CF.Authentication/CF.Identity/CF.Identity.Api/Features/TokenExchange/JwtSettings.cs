namespace CF.Identity.Api.Features.TokenExchange;

public record JwtSettings
{
    public JwtSettings(IConfiguration configuration)
    {
        configuration.GetRequiredSection("JwtSettings")
            .Bind(this);
    }

    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? SigningKey { get; set; }
}
