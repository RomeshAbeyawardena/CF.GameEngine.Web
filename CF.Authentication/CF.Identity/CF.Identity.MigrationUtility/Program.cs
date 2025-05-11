// See https://aka.ms/new-console-template for more information
using IDFCR.Utility.Shared;
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.Extensions.DependencyInjection;
using CF.Identity.Infrastructure.Features.Clients;
using Microsoft.Extensions.Configuration;

using var migrationUtility = EFMigrationUtility
    .MigrationUtility<CFIdentityDbContext>(new EFMigrationUtilityName("CF.Identity", "1.0"), args, "123dacb9-a24c-4c4d-b2c5-bf465343f8d8",
    (h, s) => s.AddBackendDependencies("IdentityDb"))
    .Extend("seed", "Seed basic data required for the database to operate correctly", SeedData);

static async Task TrySeedScopesAsync(ILogger logger, CFIdentityDbContext context, CancellationToken cancellationToke)
{
    string[] defaultScopes = ["api:read", "api:write"];

    var scopes = await context.Scopes.Where(x => !x.ClientId.HasValue && defaultScopes.Contains(x.Key)).ToListAsync(cancellationToke);

    if (scopes.All(x => defaultScopes.Contains(x.Key)))
    {
        logger.LogInformation("No scopes to seed, skipping seeding.");
        return;
    }

    var scopesToSeed = defaultScopes.Where(s => !scopes.Any(x => s == x.Key));
    foreach(var scope in scopesToSeed)
    {
        var newScope = new DbScope
        {
            Key = scope,
            Name = scope,
            Description = scope,
            Id = Guid.NewGuid()
        };
        context.Scopes.Add(newScope);
    }
}

static async Task TrySeedSystemClient(ILogger logger, CFIdentityDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken)
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
    systemClient.SecretHash = clientCredentialHasher.Hash(configuration["Seed:SystemClientSecret"] 
        ?? throw new NullReferenceException("System client secret not configured"), systemClient);

    await context.Clients.AddAsync(systemClient, cancellationToken);
}

static async Task SeedData(ILogger logger, CFIdentityDbContext context, IEnumerable<string> args, IServiceProvider serviceProvider, CancellationToken cancellationToken)
{
    logger.LogInformation("Seeding data...");

    await TrySeedScopesAsync(logger, context, cancellationToken);
    // Add your seeding logic here
    await TrySeedSystemClient(logger, context, serviceProvider, cancellationToken);

    await context.SaveChangesAsync(cancellationToken);
}

await migrationUtility.InitialiseAsync();