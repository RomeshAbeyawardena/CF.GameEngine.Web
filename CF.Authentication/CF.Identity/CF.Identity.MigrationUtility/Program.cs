// See https://aka.ms/new-console-template for more information
using IDFCR.Utility.Shared;
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Extensions;
using Microsoft.Extensions.Logging;
using CF.Identity.MigrationUtility.Verify;
using CF.Identity.MigrationUtility.Seeds;

using var migrationUtility = EFMigrationUtility
    .MigrationUtility<CFIdentityDbContext>(new EFMigrationUtilityName("CF.Identity", "1.0"), args, "123dacb9-a24c-4c4d-b2c5-bf465343f8d8",
    (h, s) => s.AddBackendDependencies("CFIdentity"))
    .Extend("--seed*", "Seed basic data required for the database to operate correctly", Seed.SeedAsync)
    .Extend("--verify-seed-data", "Verify seeded data can be decrypted or used to verify security data", VerifySeedData);

static async Task VerifySeedData(ILogger logger, CFIdentityDbContext context, IEnumerable<string> args, IServiceProvider serviceProvider, CancellationToken cancellationToken)
{
   await Verify
        .VerifyUserSeedData(logger, context, serviceProvider, cancellationToken);
}

await migrationUtility.InitialiseAsync();