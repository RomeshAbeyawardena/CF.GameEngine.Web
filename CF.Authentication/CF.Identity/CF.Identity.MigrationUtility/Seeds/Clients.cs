using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.MigrationUtility.Seeds;

internal static partial class Seed
{
    internal static async Task TrySeedSystemClientAsync(ILogger logger, CFIdentityDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        if (await context.Clients.Where(c => c.IsSystem).AnyAsync(cancellationToken))
        {
            logger.LogInformation("No clients to seed, skipping seeding.");
            return;
        }

        var systemClient = new DbClient
        {
            Reference = "system",
            Name = "System Client",
            ValidFrom = DateTimeOffset.UtcNow,
            ValidTo = DateTimeOffset.UtcNow.AddYears(100),
            IsSystem = true,
        };

        var clientCredentialHasher = serviceProvider.GetRequiredService<IClientCredentialHasher>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        systemClient.SecretHash = clientCredentialHasher.Hash(configuration["Seed:Client:SystemClientSecret"]
            ?? throw new NullReferenceException("System client secret not configured"), systemClient);

        await context.Clients.AddAsync(systemClient, cancellationToken);
    }

}
