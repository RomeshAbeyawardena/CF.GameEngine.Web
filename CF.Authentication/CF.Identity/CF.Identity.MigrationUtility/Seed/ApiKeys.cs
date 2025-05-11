using CF.Identity.Infrastructure;
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace CF.Identity.MigrationUtility.Seed;

public partial class Seed
{
    internal static async Task TrySeedApiKeyAsync(ILogger logger, CFIdentityDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        //check if client is in-flight
        var client = context.Clients.Local.FirstOrDefault(x => x.IsSystem);

        var isSystemClientInFlight = client is not null;

        client ??= await context.Clients.FirstOrDefaultAsync(x => x.IsSystem, cancellationToken)
            ?? throw new NullReferenceException("Seeding can not continue ");

        
        if (!isSystemClientInFlight && !(await context.AccessTokens.AnyAsync(x => x.ClientId == client.Id, cancellationToken)))
        {
            var randomNumberGenerator = serviceProvider.GetRequiredService<RandomNumberGenerator>();
            var jwtSettings = serviceProvider.GetRequiredService<IJwtSettings>();
            var referenceToken = JwtHelper.GenerateSecureRandomBase64( randomNumberGenerator, 32);
            var refreshToken = JwtHelper.GenerateSecureRandomBase64(randomNumberGenerator, 16);
            var newApiKey = new DbAccessToken
            {
                Id = Guid.NewGuid(),
                ClientId = client.Id,
                ReferenceToken = referenceToken,
                RefreshToken = refreshToken,
                AccessToken = JwtHelper.GenerateJwt(client, string.Join(' ', DefaultScopes)
                , jwtSettings),
                Type = "api_key",
                ValidFrom = DateTimeOffset.UtcNow,
                ValidTo = DateTimeOffset.UtcNow.AddYears(1)
            };
            context.AccessTokens.Add(newApiKey);
        }
        else
        {
            logger.LogInformation("No API keys to seed, skipping seeding.");
        }
    }
}
