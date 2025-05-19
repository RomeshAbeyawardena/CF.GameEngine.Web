using CF.Identity.Infrastructure.SqlServer;
using Microsoft.Extensions.Logging;

namespace CF.Identity.MigrationUtility.Verify;

internal static partial class Verify
{
    public static async Task VerifyClientSeedData(ILogger logger, CFIdentityDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}
