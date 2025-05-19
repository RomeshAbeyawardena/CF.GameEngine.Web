using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CF.Identity.MigrationUtility.Verify;

internal static partial class Verify
{
    public static async Task<bool> VerifyUserSeedData(ILogger logger, CFIdentityDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken)
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

            var expectedEmailAddressHmac = userCredentialProtectionProvider.HashUsingHmac(user.Client, userInfo.EmailAddress);

            var expectedUsernameHmac = userCredentialProtectionProvider.HashUsingHmac(user.Client, userInfo.Username);

            var expectedPreferredUsernameHmac = userCredentialProtectionProvider.HashUsingHmac(user.Client, userInfo.PreferredUsername);

            int issueCount = 0;
            if (expectedEmailAddressHmac != user.EmailAddressHmac)
            {
                logger.LogWarning("Email address HMAC does not match: {expectedEmailAddressHmac} != {EmailAddressHmac}", expectedEmailAddressHmac, user.EmailAddressHmac);
                issueCount++;
            }

            if (expectedUsernameHmac != user.UsernameHmac)
            {
                logger.LogWarning("Username HMAC does not match: {expectedUsernameHmac} != {UsernameHmac}", expectedUsernameHmac, user.UsernameHmac);
                issueCount++;
            }

            if (expectedPreferredUsernameHmac != user.PreferredUsernameHmac)
            {
                logger.LogWarning("Preferred username HMAC does not match: {expectedPreferredUsernameHmac} != {preferredUsernameHmac}", expectedPreferredUsernameHmac, user.PreferredUsernameHmac);
                issueCount++;
            }

            if (issueCount > 0)
            {
                logger.LogError("There are {issueCount} issues with the user data, see previous entries for details. " +
                    "This will impact filtering encrypted fields within the system until its resolved", issueCount);
            }

            var mappedUser = user.Map<UserDto>();
            userCredentialProtectionProvider.Unprotect(mappedUser, user.Client);

            if (userInfo.EmailAddress != mappedUser.EmailAddress)
            {
                var expected = userInfo.EmailAddress;
                logger.LogWarning("Email address does not match: {emailAddress} != {expected}", mappedUser.EmailAddress, expected);
                issueCount++;
            }

            if (userInfo.Username != mappedUser.Username)
            {
                var expected = userInfo.Username;
                logger.LogWarning("Username does not match: {userName} != {expected}", mappedUser.Username, expected);
                issueCount++;
            }

            if (userInfo.PreferredUsername != mappedUser.PreferredUsername)
            {
                var expected = userInfo.PreferredUsername;
                logger.LogWarning("Preferred username does not match: {preferredUserName} != {expected}", mappedUser.PreferredUsername, expected);
                issueCount++;
            }

            var hashedPassword = userCredentialProtectionProvider.Hash(userInfo.Password, user);

            if(user.HashedPassword != hashedPassword)
            {
                logger.LogWarning("Password matches the hashed password in the database, this is not expected. " +
                    "This is likely due to the password being seeded with the same value as the hashed password.");
                issueCount++;
            }

            if (issueCount == 0)
            {
                logger.LogInformation("User data verified successfully.");
            }
            else
            {
                logger.LogWarning("User data verified with errors");
            }

            return issueCount == 0;
        }
        return false;
    }
}
