using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CF.Identity.Infrastructure.SqlServer.PII;
using CF.Identity.Infrastructure.SqlServer.SPA;

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

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        var systemClient = new DbClient
        {
            Reference = "system",
            Name = "System Client",
            ValidFrom = DateTimeOffset.UtcNow,
            ValidTo = DateTimeOffset.UtcNow.AddYears(1),
            SecretHash = configuration.GetValue<string>("Seed:Client:SystemClientSecret"),
            IsSystem = true,
        };

        var clientProtection = serviceProvider.GetRequiredService<IClientSpaProtection>();
        await context.Clients.AddAsync(systemClient, cancellationToken);
        clientProtection.Protect(systemClient);
    }

}
