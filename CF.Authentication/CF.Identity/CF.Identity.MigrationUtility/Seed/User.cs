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

            var isInFlight = client is null;
            if (isInflight)
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
        userCredentialProtectionProvider.Protect(user);

        context.Users.Add(user);
    }
}
