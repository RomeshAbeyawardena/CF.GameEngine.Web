using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace CF.Identity.MigrationUtility.Seeds;

internal static partial class Seed
{
    public static IEnumerable<string> DefaultScopes => ["api:read", "api:write"];
    internal static async Task TrySeedScopesAsync(ILogger logger, CFIdentityDbContext context, CancellationToken cancellationToken)
    {
        var scopes = await context.Scopes.Where(x => !x.ClientId.HasValue && DefaultScopes.Contains(x.Key)).ToListAsync(cancellationToken);

        var missingScopes = DefaultScopes.Except(scopes.Select(s => s.Key)).ToList();
        if (missingScopes.Count < 1)
        {
            logger.LogInformation("No scopes to seed, skipping seeding.");
            return;
        }

        foreach (var scope in missingScopes)
        {
            var newScope = new DbScope
            {
                IsPrivileged = true,
                Key = scope,
                Name = scope,
                Description = scope
            };

            context.Scopes.Add(newScope);
        }
    }
}