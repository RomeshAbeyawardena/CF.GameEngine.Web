using CF.Identity.Infrastructure.SqlServer;
using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.SPA;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;

namespace CF.Identity.MigrationUtility.Seeds;

internal static partial class Seed
{
    internal static async Task TrySeedSystemClientAsync(ILogger logger, CFIdentityDbContext context, IEnumerable<string> args,
        IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        if (await context.Clients.Where(c => c.IsSystem).AnyAsync(cancellationToken))
        {
            logger.LogInformation("No clients to seed, skipping seeding.");
            return;
        }

        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        var reference = configuration.GetValue<string>("Seed:Client:Reference") ?? throw new NullReferenceException("Seed client reference missing");
        var secret = configuration.GetValue<string>("Seed:Client:SystemClientSecret");
        var systemClient = new DbClient
        {
            Reference = reference,
            Name = configuration.GetValue<string>("Seed:Client:Name") ?? throw new NullReferenceException("Seed client name missing"),
            ValidFrom = DateTimeOffset.UtcNow,
            ValidTo = DateTimeOffset.UtcNow.AddYears(1),
            SecretHash = secret,
            IsSystem = true,
        };

        var clientProtection = serviceProvider.GetRequiredService<IClientSpaProtection>();
        await context.Clients.AddAsync(systemClient, cancellationToken);
        clientProtection.Protect(systemClient);

        var output = new StringBuilder();
        var encoding = serviceProvider.GetRequiredService<Encoding>();
        var xAuth = Convert.ToBase64String(encoding.GetBytes($"{reference}:{secret}"));
        output.AppendLine($"🔒 {xAuth}");

        if (args.Any(x => x.Equals("--output", StringComparison.InvariantCultureIgnoreCase)))
        {
            await File.WriteAllTextAsync("ApiKey.txt", output.ToString(), cancellationToken);
        }
    }

}
