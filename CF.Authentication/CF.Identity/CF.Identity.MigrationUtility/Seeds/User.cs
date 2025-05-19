using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.Transforms;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CF.Identity.MigrationUtility.Seeds;

internal static partial class Seed
{
    public static async Task TrySeedUsersAsync(ILogger logger, CFIdentityDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var client = await context.Clients.FirstOrDefaultAsync(c => c.IsSystem, cancellationToken);
        bool isInflight = false;
        if (client is null)
        {
            client = context.Clients.Local.FirstOrDefault(c => c.IsSystem);

            isInflight = client is not null;
            if (!isInflight)
            {
                throw new Exception("System client not found");
            }
        }

        if (!isInflight && await context.Users.AnyAsync(c => c.ClientId == client!.Id && c.IsSystem, cancellationToken))
        {
            logger.LogInformation("No users to seed, skipping seeding.");
            return;
        }

        var userInfo = serviceProvider.GetRequiredService<UserInfo>();

        var userDto = new UserDto
        {
            EmailAddress = userInfo.EmailAddress,
            Username = userInfo.Username,
            PreferredUsername = userInfo.PreferredUsername,
            // The initial password will be hashed/encrypted by Protect(), this is a plain-text seed value.
            HashedPassword = userInfo.Password,
            Firstname = userInfo.Firstname,
            Lastname = userInfo.Lastname,
            PrimaryTelephoneNumber = userInfo.PrimaryTelephoneNumber,
            IsSystem = true,
        };

        var userCredentialProtectionProvider = serviceProvider.GetRequiredService<IUserCredentialProtectionProvider>();
        userCredentialProtectionProvider.Protect(userDto, client!, out var hmac);

        var user = await UserTransformer.Transform(userDto, context, hmac, cancellationToken);

        if (!isInflight)
        {
            user.ClientId = client!.Id;
        }
        else
        {
            user.Client = client!;
        }

        List<(bool, DbScope)> scopesToAdd = [];

        context.Scopes.Local.ForEach(s => scopesToAdd.Add((true, s)));

        if (!isInflight)
        {
            //include existing scopes for the client
            var scopes = await context.Scopes.Where(x => !x.ClientId.HasValue || x.ClientId == client!.Id).ToListAsync(cancellationToken);
            scopes.ForEach(s => scopesToAdd.Add(new(false, s)));
        }

        foreach(var (inflight, scope) in scopesToAdd)
        {
            if(scopesToAdd.Any(b => b.Item2.Name == scope.Name))
            {
                continue;
            }

            var userScope = new DbUserScope
            {
                User = user
            };

            if (inflight)
            {
                userScope.Scope = scope;
            }
            else
            {
                userScope.ScopeId = scope.Id;
            }

            user.UserScopes.Add(userScope);
        }

        context.Users.Add(user);
    }
}
