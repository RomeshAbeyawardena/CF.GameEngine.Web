// See https://aka.ms/new-console-template for more information
using IDFCR.Utility.Shared;
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Extensions;

using var migrationUtility = EFMigrationUtility
    .MigrationUtility<CFIdentityDbContext>(new EFMigrationUtilityName("CF.Identity", "1.0"), args, "123dacb9-a24c-4c4d-b2c5-bf465343f8d8",
    (h, s) => s.AddBackendDependencies("IdentityDb"));

await migrationUtility.InitialiseAsync();