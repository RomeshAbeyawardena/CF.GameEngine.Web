using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CF.Identity.MigrationUtility.Verify;

internal partial class Verify
{
    public static async Task VerifyUserSeedData(ILogger logger, CFIdentityDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var user = await context.Users.AsNoTracking().Include(x => x.Client)
        .Include(x => x.FirstCommonName)
        .Include(x => x.MiddleCommonName)
        .Include(x => x.LastCommonName)
        .FirstOrDefaultAsync(x => x.IsSystem, cancellationToken);

        if (user is not null)
        {
            var userCredentialProtectionProvider = serviceProvider.GetRequiredService<IUserCredentialProtectionProvider>();
            var userInfo = serviceProvider.GetRequiredService<UserInfo>();

            var emailAddressHmac = userCredentialProtectionProvider.HashUsingHmac(user.Client, userInfo.EmailAddress);

            var usernameHmac = userCredentialProtectionProvider.HashUsingHmac(user.Client, userInfo.Username);

            var preferredUsernameHmac = userCredentialProtectionProvider.HashUsingHmac(user.Client, userInfo.PreferredUsername);

            if (emailAddressHmac != user.EmailAddressHmac)
            {
                logger.LogWarning("Email address HMAC does not match: {emailAddressHmac} != {emailAddressHmac}", emailAddressHmac, user.EmailAddressHmac);
            }

            if (usernameHmac != user.UsernameHmac)
            {
                logger.LogWarning("Username HMAC does not match: {usernameHmac} != {usernameHmac}", usernameHmac, user.UsernameHmac);
            }

            if (preferredUsernameHmac != user.PreferredUsernameHmac)
            {
                logger.LogWarning("Preferred username HMAC does not match: {preferredUsernameHmac} != {preferredUsernameHmac}", preferredUsernameHmac, user.PreferredUsernameHmac);
            }

            var mappedUser = user.Map<UserDto>();
            userCredentialProtectionProvider.Unprotect(mappedUser, user.Client);

            if (userInfo.EmailAddress != mappedUser.EmailAddress)
            {
                logger.LogWarning("Email address does not match: {emailAddress} != {emailAddress}", userInfo.EmailAddress, mappedUser.EmailAddress);
            }

            if (userInfo.Username != mappedUser.Username)
            {
                logger.LogWarning("Username does not match: {userName} != {userName}", userInfo.Username, mappedUser.Username);
            }

            if (userInfo.PreferredUsername != mappedUser.PreferredUsername)
            {
                logger.LogWarning("Preferred username does not match: {preferredUserName} != {preferredUserName}", userInfo.PreferredUsername, mappedUser.PreferredUsername);
            }

        }
    }
}
