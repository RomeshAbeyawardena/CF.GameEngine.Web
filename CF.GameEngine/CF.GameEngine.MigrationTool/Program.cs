using CF.GameEngine.Infrastructure.SqlServer;
using CF.GameEngine.Infrastructure.SqlServer.Extensions;
using IDFCR.Utility.Shared;

using var utility = EFMigrationUtility.MigrationUtility<CFGameEngineDbContext>(
    new EFMigrationUtilityName("CF.GameEngine", "1.0.0"), args,
    "9315e900-337b-4d8c-9599-6b6a66b9af5e",
    (hostContext, services) =>
    {
        services.AddBackendDependencies("GameEngineDb");
    });

await utility.InitialiseAsync();