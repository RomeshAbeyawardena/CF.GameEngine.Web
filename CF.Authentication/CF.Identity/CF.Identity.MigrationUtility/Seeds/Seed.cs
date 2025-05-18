using CF.Identity.Infrastructure.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CF.Identity.MigrationUtility.Seeds;

internal static partial class Seed
{
    public static async Task SeedAsync(ILogger logger, CFIdentityDbContext context, IEnumerable<string> args, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        logger.LogInformation("Seeding data...");

        await TrySeedScopesAsync(logger, context, cancellationToken);

        await TrySeedSystemClientAsync(logger, context, serviceProvider, cancellationToken);

        if (args.Any(x => x.StartsWith("--seed:", StringComparison.InvariantCultureIgnoreCase)))
        {
            IHostEnvironment hostEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();
            if (!hostEnvironment.IsDevelopment())
            {
                logger.LogWarning("Test data seeding is not recommended out of development environments.");
            }
        }

        var seedAll = args.Any(x => x.Equals("--seed:all", StringComparison.InvariantCultureIgnoreCase));

        if (seedAll || args.Any(x => x.Equals("seed:user", StringComparison.InvariantCultureIgnoreCase)))
        {
            await TrySeedUsersAsync(logger, context, serviceProvider, cancellationToken);
        }

        if (seedAll || args.Any(x => x.Equals("seed:api_key", StringComparison.InvariantCultureIgnoreCase)))
        {
            await Seed.TrySeedApiKeyAsync(logger, context, serviceProvider, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
