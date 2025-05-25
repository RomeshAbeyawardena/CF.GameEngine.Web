using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CF.Identity.MigrationUtility.Seeds;

internal static partial class Seed
{
    internal static async Task TrySeedScopesAsync(ILogger logger, CFIdentityDbContext context, CancellationToken cancellationToken)
    {
        if(Roles is null)
        {
            return;
        }

        var scopesToAdd = Roles.Select(x => x.Key);
        var scopes = await context.Scopes.Where(x => !x.ClientId.HasValue && scopesToAdd.Contains(x.Key)).ToListAsync(cancellationToken);

        var missingScopes = scopesToAdd.Except(scopes.Select(s => s.Key)).ToList();
        if (missingScopes.Count < 1)
        {
            logger.LogInformation("No scopes to seed, skipping seeding.");
            return;
        }

        var scopeDictionary = Roles.ToDictionary(x => x.Key, x => x);

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
                Name = existingScope.DisplayName ?? scope,
                Description = existingScope.Description ?? scope
            };

            context.Scopes.Add(newScope);
        }
    }
}