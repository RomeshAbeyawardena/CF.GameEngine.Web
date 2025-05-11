// See https://aka.ms/new-console-template for more information
using IDFCR.Utility.Shared;
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Extensions;
using Microsoft.Extensions.Logging;
using CF.Identity.MigrationUtility.Seed;

using var migrationUtility = EFMigrationUtility
    .MigrationUtility<CFIdentityDbContext>(new EFMigrationUtilityName("CF.Identity", "1.0"), args, "123dacb9-a24c-4c4d-b2c5-bf465343f8d8",
    (h, s) => s.AddBackendDependencies("IdentityDb"))
    .Extend("seed", "Seed basic data required for the database to operate correctly", SeedData);

static async Task SeedData(ILogger logger, CFIdentityDbContext context, IEnumerable<string> args, IServiceProvider serviceProvider, CancellationToken cancellationToken)
{
    logger.LogInformation("Seeding data...");

    await Seed.TrySeedScopesAsync(logger, context, cancellationToken);
    
    await Seed.TrySeedSystemClient(logger, context, serviceProvider, cancellationToken);

    if(args.Any(x => x.Equals("seed:api_key", StringComparison.InvariantCultureIgnoreCase)))
    {
        await Seed.TrySeedApiKeyAsync(logger, context, serviceProvider, cancellationToken);
    }

    await context.SaveChangesAsync(cancellationToken);
}

await migrationUtility.InitialiseAsync();