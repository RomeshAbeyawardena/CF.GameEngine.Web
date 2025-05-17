using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.Transforms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CF.Identity.MigrationUtility.Seed;

public static partial class Seed
{
    public static async Task TrySeedUsers(ILogger logger, CFIdentityDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken)
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

        var userDto = new UserDto
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

        var userCredentialProtectionProvider = serviceProvider.GetRequiredService<IUserCredentialProtectionProvider>();
        userCredentialProtectionProvider.Protect(userDto, client!);

        var user = await UserTransformer.Transform(userDto, context, cancellationToken);

        if (!isInflight)
        {
            user.ClientId = client!.Id;
        }
        else
        {
            user.Client = client!;
        }

        context.Users.Add(user);
    }
}
