// See https://aka.ms/new-console-template for more information
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Extensions;
using CF.Identity.MigrationUtility;
using CF.Identity.MigrationUtility.Seeds;
using CF.Identity.MigrationUtility.Verify;
using IDFCR.Utility.Shared;
using IDFCR.Utility.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using var migrationUtility = EFMigrationUtility
    .MigrationUtility<CFIdentityDbContext>(new EFMigrationUtilityName("CF.Identity", "1.0"), args, "123dacb9-a24c-4c4d-b2c5-bf465343f8d8",
    (h, s) => s.AddSingleton<UserInfo>()
        .AddBackendDependencies("CFIdentity"))
    .Extend("--seed*", "Seed basic data required for the database to operate correctly", Seed.SeedAsync)
    .Extend("--verify-seed-data", "Verify seeded data can be decrypted or used to verify security data", VerifySeedData);

static async Task<MigrationResult> VerifySeedData(ILogger logger, CFIdentityDbContext context, IEnumerable<string> args, 
    IServiceProvider serviceProvider, CancellationToken cancellationToken)
{
   await Verify
        .VerifyUserSeedData(logger, context, serviceProvider, cancellationToken);

    return new MigrationResult(nameof(VerifySeedData), MigrationStatus.Completed);
}

await migrationUtility.InitialiseAsync();
Console.WriteLine(new string('-', Console.BufferWidth));
Console.WriteLine(migrationUtility.Results.ToReport());
Environment.Exit(migrationUtility.Results.ToExitCode());