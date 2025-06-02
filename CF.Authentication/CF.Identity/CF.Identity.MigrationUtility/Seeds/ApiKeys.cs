using CF.Identity.Infrastructure;
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.SPA;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace CF.Identity.MigrationUtility.Seeds;

internal static partial class Seed
{
    internal static async Task TrySeedApiKeyAsync(ILogger logger, CFIdentityDbContext context, IEnumerable<string> args, 
        IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        if(Roles is null)
        {
            return;
        }

        //check if client is in-flight
        var client = context.Clients.Local.FirstOrDefault(x => x.IsSystem);

        var isSystemClientInFlight = client is not null;

        client ??= await context.Clients.FirstOrDefaultAsync(x => x.IsSystem, cancellationToken)
            ?? throw new NullReferenceException("Seeding can not continue ");

        
        if (!await context.AccessTokens.AnyAsync(x => x.ClientId == client.Id, cancellationToken))
        {
            var randomNumberGenerator = serviceProvider.GetRequiredService<RandomNumberGenerator>();
            var jwtSettings = serviceProvider.GetRequiredService<IJwtSettings>();
            logger.LogInformation("{fullName}", jwtSettings.GetType().FullName);
            
            var referenceToken = JwtHelper.GenerateSecureRandomBase64(randomNumberGenerator, 32);
            
            var refreshToken = JwtHelper.GenerateSecureRandomBase64(randomNumberGenerator, 16);
            
            var validity = DateTimeOffset.UtcNow.AddYears(1);
            var newApiKey = new DbAccessToken
            {
                Id = Guid.NewGuid(),
                ReferenceToken = referenceToken,
                RefreshToken = refreshToken,
                AccessToken = JwtHelper.GenerateJwt(client, string.Join(' ', Roles.Where(x => x.IsPrivileged).Select(x => x.Key))
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

            var accessTokenProtection = serviceProvider.GetRequiredService<IAccessTokenSpaProtection>();
            accessTokenProtection.Client = client;
            accessTokenProtection.Protect(newApiKey);

            var outputBuilder = new StringBuilder();
            outputBuilder.AppendLine($"✅ API key successfully seeded for system client.");
            outputBuilder.AppendLine($"🔑 Reference Token: {referenceToken}");
            outputBuilder.AppendLine($"♻️ Refresh Token: {refreshToken}");
            outputBuilder.AppendLine("👉 Use the Reference Token in Postman or curl as the `Authorization: Bearer <token>` header.");
            var output = outputBuilder.ToString();
            logger.LogInformation("{output}", output);

            if (args.Any(x => x.Equals("--output", StringComparison.InvariantCultureIgnoreCase)))
            {
                await File.WriteAllTextAsync("ApiKey.txt", output, cancellationToken);
            }
        }
        else
        {
            logger.LogInformation("No API keys to seed, skipping seeding.");
        }
    }
}
