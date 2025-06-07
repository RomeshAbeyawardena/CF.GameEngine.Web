using CF.Identity.Infrastructure.Features.Users;
using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.PII;
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
            var userCredentialProtectionProvider = serviceProvider.GetRequiredService<IUserPIIProtection>();
            userCredentialProtectionProvider.Client = user.Client;
            var userInfo = serviceProvider.GetRequiredService<UserInfo>();

            int issueCount = 0;
            if (!userCredentialProtectionProvider.VerifyHmacUsing(user, x => x.EmailAddress, userInfo.EmailAddress))
            {
                logger.LogWarning("Email address HMAC does not match");
                issueCount++;
            }

            if (!userCredentialProtectionProvider.VerifyHmacUsing(user, x => x.Username, userInfo.Username))
            {
                logger.LogWarning("Username HMAC does not match");
                issueCount++;
            }

            if (!userCredentialProtectionProvider.VerifyHmacUsing(user, x => x.PreferredUsername, userInfo.PreferredUsername))
            {
                logger.LogWarning("Preferred username HMAC does not match");
                issueCount++;
            }

            if (issueCount > 0)
            {
                logger.LogError("There are {issueCount} issues with the user data, see previous entries for details. " +
                    "This will impact filtering encrypted fields within the system until its resolved", issueCount);
            }

            userCredentialProtectionProvider.Unprotect(user);

            var mappedUser = user.Map<UserDto>();
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

            if (!userCredentialProtectionProvider.VerifyHashUsing(user, x => x.HashedPassword, userInfo.Password))
            {
                logger.LogWarning("Password does not match the value stored in the database");
                issueCount++;
            }

            var hasNoIssues = issueCount == 0;

            if (hasNoIssues)
            {
                logger.LogInformation("User data has been successfully encrypted and decrypted");
            }

            return hasNoIssues;
        }
        return false;
    }
}
