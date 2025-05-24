using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CF.Identity.MigrationUtility.Seeds;

internal static partial class Seed
{
    internal static async Task TrySeedScopesAsync(ILogger logger, IEnumerable<IRoleDescriptor> discoveredScopes, CFIdentityDbContext context, CancellationToken cancellationToken)
    {
        var scopesToAdd = discoveredScopes.Select(x => x.Key);
        var scopes = await context.Scopes.Where(x => !x.ClientId.HasValue && scopesToAdd.Contains(x.Key)).ToListAsync(cancellationToken);

        var missingScopes = scopesToAdd.Except(scopes.Select(s => s.Key)).ToList();
        if (missingScopes.Count < 1)
        {
            logger.LogInformation("No scopes to seed, skipping seeding.");
            return;
        }

        var scopeDictionary = scopes.ToDictionary(x => x.Key, x => x);

        foreach (var scope in missingScopes)
        {
            if(!scopeDictionary.TryGetValue(scope, out var existingScope))
            {
                continue;
            }

            var newScope = new DbScope
            {
                IsPrivileged = existingScope.IsPrivileged,
                Key = scope,
                Name = scope,
                Description = scope
            };

            context.Scopes.Add(newScope);
        }
    }
}