using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.MigrationUtility.Seed;

public static partial class Seed
{
    internal static async Task TrySeedScopesAsync(ILogger logger, CFIdentityDbContext context, CancellationToken cancellationToke)
    {
        string[] defaultScopes = ["api:read", "api:write"];

        var scopes = await context.Scopes.Where(x => !x.ClientId.HasValue && defaultScopes.Contains(x.Key)).ToListAsync(cancellationToke);

        if (scopes.All(x => defaultScopes.Contains(x.Key)))
        {
            logger.LogInformation("No scopes to seed, skipping seeding.");
            return;
        }

        var scopesToSeed = defaultScopes.Where(s => !scopes.Any(x => s == x.Key));
        foreach (var scope in scopesToSeed)
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
}