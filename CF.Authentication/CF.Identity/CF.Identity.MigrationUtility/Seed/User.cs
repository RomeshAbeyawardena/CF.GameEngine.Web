using CF.Identity.Infrastructure.Features.Clients;
using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace CF.Identity.MigrationUtility.Seed;

public static partial class Seed
{
    public static async Task SeedUsers(ILogger logger, CFIdentityDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken)
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

        var user = new DbUser
        {
            EmailAddress = "admin@identity.co",
            Username = "admin",
            PreferredUsername = "admin",
            // The initial password will be hashed/encrypted by Protect(), this is a plain-text seed value.
            HashedPassword = "@dmin-123!",
            Firstname = "Admin",
            LastName = "User",
            IsSystem = true,
        };

        if (!isInflight)
        {
            user.ClientId = client!.Id;
        }
        else
        {
            user.Client = client!;
        }

        var userCredentialProtectionProvider = serviceProvider.GetRequiredService<IUserCredentialProtectionProvider>();
        userCredentialProtectionProvider.Protect(user, isInflight ? client : null);

        context.Users.Add(user);
    }
}
