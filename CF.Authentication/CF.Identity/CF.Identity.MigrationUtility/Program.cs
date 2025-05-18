// See https://aka.ms/new-console-template for more information
using IDFCR.Utility.Shared;
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Extensions;
using Microsoft.Extensions.Logging;
using CF.Identity.MigrationUtility.Seed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.MigrationUtility;
using System.ComponentModel.DataAnnotations;

using var migrationUtility = EFMigrationUtility
    .MigrationUtility<CFIdentityDbContext>(new EFMigrationUtilityName("CF.Identity", "1.0"), args, "123dacb9-a24c-4c4d-b2c5-bf465343f8d8",
    (h, s) => s.AddBackendDependencies("CFIdentity"))
    .Extend("--seed*", "Seed basic data required for the database to operate correctly", SeedData)
    .Extend("--verify-seed-data", "Verify seeded data can be decrypted or used to verify security data", VerifySeedData);

static async Task VerifySeedData(ILogger logger, CFIdentityDbContext context, IEnumerable<string> args, IServiceProvider serviceProvider, CancellationToken cancellationToken)
{
    var user = await context.Users.AsNoTracking().Include(x => x.Client)
        .Include(x => x.FirstCommonName)
        .Include(x => x.MiddleCommonName)
        .Include(x => x.LastCommonName)
        .FirstOrDefaultAsync(x => x.IsSystem, cancellationToken);

    if(user is not null)
    {
        var userCredentialProtectionProvider = serviceProvider.GetRequiredService<IUserCredentialProtectionProvider>();
        var userInfo = serviceProvider.GetRequiredService<UserInfo>();

        var emailAddressHmac = userCredentialProtectionProvider.HashUsingHmac(user.Client, userInfo.EmailAddress);

        var usernameHmac = userCredentialProtectionProvider.HashUsingHmac(user.Client, userInfo.Username);

        var preferredUsernameHmac = userCredentialProtectionProvider.HashUsingHmac(user.Client, userInfo.PreferredUsername);

        if(emailAddressHmac != user.EmailAddressHmac)
        {
            logger.LogWarning("Email address HMAC does not match: {emailAddressHmac} != {emailAddressHmac}", emailAddressHmac, user.EmailAddressHmac);
        }

        if(usernameHmac != user.UsernameHmac)
        {
            logger.LogWarning("Username HMAC does not match: {usernameHmac} != {usernameHmac}", usernameHmac, user.UsernameHmac);
        }

        if(preferredUsernameHmac != user.PreferredUsernameHmac)
        {
            logger.LogWarning("Preferred username HMAC does not match: {preferredUsernameHmac} != {preferredUsernameHmac}", preferredUsernameHmac, user.PreferredUsernameHmac);
        }

        var mappedUser = user.Map<UserDto>();
        userCredentialProtectionProvider.Unprotect(mappedUser, user.Client);

        if(userInfo.EmailAddress != mappedUser.EmailAddress)
        {
            logger.LogWarning("Email address does not match: {emailAddress} != {emailAddress}", userInfo.EmailAddress, mappedUser.EmailAddress);
        }

        if(userInfo.Username != mappedUser.Username)
        {
            logger.LogWarning("Username does not match: {userName} != {userName}", userInfo.Username, mappedUser.Username);
        }

        if(userInfo.PreferredUsername != mappedUser.PreferredUsername)
        {
            logger.LogWarning("Preferred username does not match: {preferredUserName} != {preferredUserName}", userInfo.PreferredUsername, mappedUser.PreferredUsername);
        }

    }
}

static async Task SeedData(ILogger logger, CFIdentityDbContext context, IEnumerable<string> args, IServiceProvider serviceProvider, CancellationToken cancellationToken)
{
    logger.LogInformation("Seeding data...");

    await Seed.TrySeedScopesAsync(logger, context, cancellationToken);
    
    await Seed.TrySeedSystemClient(logger, context, serviceProvider, cancellationToken);

    if (args.Any(x => x.StartsWith("--seed:", StringComparison.InvariantCultureIgnoreCase)))
    {
        IHostEnvironment hostEnvironment = serviceProvider.GetRequiredService<IHostEnvironment>();
        if (!hostEnvironment.IsDevelopment())
        {
            logger.LogWarning("Test data seeding is not recommended out of development environments.");
        }
    }

    var seedAll = args.Any(x => x.Equals("--seed:all", StringComparison.InvariantCultureIgnoreCase));

    if (seedAll || args.Any(x => x.Equals("seed:user", StringComparison.InvariantCultureIgnoreCase)))
    {
        await Seed.TrySeedUsers(logger, context, serviceProvider, cancellationToken);
    }

    if (seedAll || args.Any(x => x.Equals("seed:api_key", StringComparison.InvariantCultureIgnoreCase)))
    {
        await Seed.TrySeedApiKeyAsync(logger, context, serviceProvider, cancellationToken);
    }

    await context.SaveChangesAsync(cancellationToken);
}

await migrationUtility.InitialiseAsync();