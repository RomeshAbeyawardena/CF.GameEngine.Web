using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        //update any existing scopes with display names or descriptions that have been updated by the model
        foreach (var (key, info) in scopeDictionary)
        {
            bool hasDisplayName = !string.IsNullOrWhiteSpace(info.DisplayName),
                 hasDescription = !string.IsNullOrWhiteSpace(info.Description);

            if(!hasDisplayName && !hasDescription)
            {
                continue; // no need to update if both are null or empty
            }

            var scope = await context.Scopes.FirstOrDefaultAsync(s => 
            s.Key == key && (s.Name != info.DisplayName || s.Description != info.Description), 
            cancellationToken);

            if (scope is null)
            {
                continue;
            }
            //only touch fields that need updating!
            if (hasDisplayName) 
            {
                scope.Name = info.DisplayName!;
            }

            if (hasDescription)
            {
                scope.Description = info.Description!;
            }
        }


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