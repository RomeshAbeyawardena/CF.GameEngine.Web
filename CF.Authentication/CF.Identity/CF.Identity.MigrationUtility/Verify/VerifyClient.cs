using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.SPA;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CF.Identity.MigrationUtility.Verify;

internal static partial class Verify
{
    public static async Task<bool> VerifyClientSeedData(ILogger logger, CFIdentityDbContext context, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var client = await context.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.IsSystem, cancellationToken);

        if (client is null)
        {
            logger.LogError("No system client found in the database. Please ensure that the system client has been seeded correctly.");
            return false;
        }

        var clientProtection = serviceProvider.GetRequiredService<IClientSpaProtection>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var expectedValue = configuration.GetValue<string>("Seed:Client:SystemClientSecret") ?? throw new NullReferenceException("");

        var result = clientProtection.VerifyHashUsing(client, x => x.SecretHash, expectedValue);

        var response = $"Client secret hash verification result: {result}";

        if (!result)
        {
            logger.LogError("Client secret hash does not match the expected value. Please check the seed configuration.");
        }
        else
        {
            logger.LogInformation("Client secret hash matches the expected value.");
        }

        return result;
    }
}
