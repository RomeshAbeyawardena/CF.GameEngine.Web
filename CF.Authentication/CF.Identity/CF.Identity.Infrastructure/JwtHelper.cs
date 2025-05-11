using CF.Identity.Infrastructure.Features.Clients;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CF.Identity.Infrastructure;

public static class JwtHelper
{
    public static string GenerateSecureRandomBase64(RandomNumberGenerator rng, int sizeInBytes)
    {
        var buffer = new byte[sizeInBytes];
        rng.GetNonZeroBytes(buffer);
        return Convert.ToBase64String(buffer);
    }

    public static string GenerateJwt(IClient client, string scope, IJwtSettings jwtSettings)
    {
        //TODO come from configuration shared with consumer. /jwks.json
        var claims = new[]
        {
         new Claim("sub", client.Reference),
         new Claim("scope", scope),
         new Claim("client_id", client.Reference)
     };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey ?? throw new ArgumentException(nameof(jwtSettings))));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
