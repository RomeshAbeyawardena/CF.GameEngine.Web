using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.SPA;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CF.Identity.MigrationUtility.Verify;

internal static partial class Verify
{
    public static async Task<bool> VerifyAccessToken(ILogger logger, CFIdentityDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        Console.WriteLine(@"This will be done interactively, and should not be run from a non-interactive context.
If you are running this in a non-interactive context, please remove the --interactive flag.");

        Console.Write("Access token: ");
        var accessToken = Console.ReadLine();
        Console.WriteLine("Refresh token");
        var refreshToken = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
        {
            logger.LogError("Access token or refresh token cannot be empty.");
            return false;
        }

        var foundAccessToken = await context.AccessTokens.AsNoTracking()
            .Include(x => x.Client)
            .FirstOrDefaultAsync(x => x.Client.IsSystem, cancellationToken);

        if (foundAccessToken is null)
        {
            logger.LogError("No system access token found in the database. Please ensure that the access token has been seeded correctly.");
            return false;
        }

        var accessTokenProtection = serviceProvider.GetRequiredService<IAccessTokenSpaProtection>();
        accessTokenProtection.Client = foundAccessToken.Client;

        var results = new List<bool>();

        results.AddRange(accessTokenProtection.VerifyHashUsing(foundAccessToken, x => x.ReferenceToken, accessToken)
            ,accessTokenProtection.VerifyHashUsing(foundAccessToken, x => x.RefreshToken, refreshToken));

        return results.TrueForAll(x => x);
    }
}
