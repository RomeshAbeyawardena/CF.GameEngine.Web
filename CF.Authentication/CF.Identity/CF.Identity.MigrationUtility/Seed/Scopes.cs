using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.MigrationUtility.Seed;

public static partial class Seed
{
    public static IEnumerable<string> DefaultScopes => ["api:read", "api:write"];
    internal static async Task TrySeedScopesAsync(ILogger logger, CFIdentityDbContext context, CancellationToken cancellationToken)
    {
        var scopes = await context.Scopes.Where(x => !x.ClientId.HasValue && DefaultScopes.Contains(x.Key)).ToListAsync(cancellationToken);

        if (scopes.All(x => DefaultScopes.Contains(x.Key)))
        {
            logger.LogInformation("No scopes to seed, skipping seeding.");
            return;
        }

        var scopesToSeed = DefaultScopes.Where(s => !scopes.Any(x => s == x.Key));
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