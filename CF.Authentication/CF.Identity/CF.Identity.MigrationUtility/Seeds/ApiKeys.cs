using CF.Identity.Infrastructure;
using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace CF.Identity.MigrationUtility.Seeds;

internal static partial class Seed
{
    internal static async Task TrySeedApiKeyAsync(ILogger logger, CFIdentityDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        //check if client is in-flight
        var client = context.Clients.Local.FirstOrDefault(x => x.IsSystem);

        var isSystemClientInFlight = client is not null;

        client ??= await context.Clients.FirstOrDefaultAsync(x => x.IsSystem, cancellationToken)
            ?? throw new NullReferenceException("Seeding can not continue ");

        
        if (!await context.AccessTokens.AnyAsync(x => x.ClientId == client.Id, cancellationToken))
        {
            var randomNumberGenerator = serviceProvider.GetRequiredService<RandomNumberGenerator>();
            var jwtSettings = serviceProvider.GetRequiredService<IJwtSettings>();
            logger.LogInformation(jwtSettings.GetType().FullName);
            var clientCredentialHasher = serviceProvider.GetRequiredService<IClientCredentialHasher>();

            var referenceToken = JwtHelper.GenerateSecureRandomBase64(randomNumberGenerator, 32);
            var hashedReferenceToken = clientCredentialHasher.Hash(referenceToken, client);

            var refreshToken = JwtHelper.GenerateSecureRandomBase64(randomNumberGenerator, 16);
            var hashedRefreshToken = clientCredentialHasher.Hash(refreshToken, client);

            var validity = DateTimeOffset.UtcNow.AddYears(1);
            var newApiKey = new DbAccessToken
            {
                Id = Guid.NewGuid(),
                ReferenceToken = hashedReferenceToken,
                RefreshToken = hashedRefreshToken,
                AccessToken = JwtHelper.GenerateJwt(client, string.Join(' ', DefaultScopes)
                , jwtSettings, validity.DateTime),
                Type = "api_key",
                ValidFrom = DateTimeOffset.UtcNow,
                ValidTo = validity
            };

            if (isSystemClientInFlight)
            {
                newApiKey.Client = client;
            }
            else
            {
                newApiKey.ClientId = client.Id;
            }

            var user = context.Users.Local.FirstOrDefault(u => u.IsSystem);
            var userIsInFlight = user is not null;
            if (!userIsInFlight)
            {
                user = await context.Users.FirstOrDefaultAsync(u => u.IsSystem, cancellationToken) ?? throw new NullReferenceException("Seeding can not complete");
                newApiKey.UserId = user.Id;
            }
            else
            {
                newApiKey.User = user!;
            }
            
            context.AccessTokens.Add(newApiKey);

            logger.LogInformation("✅ API key successfully seeded for system client.");
            logger.LogInformation("🔑 Reference Token: {referenceToken}", referenceToken);
            logger.LogInformation("♻️ Refresh Token: {refreshToken}", refreshToken);
            logger.LogInformation("👉 Use the Reference Token in Postman or curl as the `Authorization: Bearer <token>` header.");

        }
        else
        {
            logger.LogInformation("No API keys to seed, skipping seeding.");
        }
    }
}
