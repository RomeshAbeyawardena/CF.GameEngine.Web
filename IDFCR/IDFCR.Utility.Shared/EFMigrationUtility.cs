using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IDFCR.Utility.Shared;

public record EFMigrationUtilityName(string Name, string Version);

public class EFMigrationUtility<TDbContext>(EFMigrationUtilityName utilityName, IEnumerable<string> args, string userSecretId, Action<HostBuilderContext, IServiceCollection> configureServices)
    where TDbContext : DbContext
{
    public async Task InitialiseAsync(CancellationToken cancellationToken = default)
    {
        using var host = new HostBuilder()
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddUserSecrets(userSecretId);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(TimeProvider.System);
                services.AddSingleton<ITimeOfDayProvider, TimeOfDayProvider>();
                configureServices(hostContext, services);
            }).Build();

        try
        {
            Console.WriteLine($"{utilityName.Name} Migration tool v{utilityName.Version}");

            var services = host.Services;
            var timeOfDayProvider = services.GetRequiredService<ITimeOfDayProvider>();
            var context = services.GetRequiredService<TDbContext>();

            var timeOfDay = timeOfDayProvider.GetTimeOfDay();
            Console.WriteLine("{0}", timeOfDay);

            if (args.Any(a => a.Equals("--verify-connection")))
            {
                if (await context.Database.CanConnectAsync(cancellationToken))
                {
                    Console.WriteLine("Database is connected.");
                }
                else
                {
                    Console.WriteLine("Database is not connected, if this is a server related issue the following actions will fail{0}\t* Migrations{0}\t* Seeding", Environment.NewLine);
                }
            }

            if (args.Any(a => a.Equals("--list", StringComparison.CurrentCultureIgnoreCase)))
            {
                var pending = await context.Database.GetPendingMigrationsAsync(cancellationToken);

                if (pending.Any())
                {
                    Console.WriteLine("Pending migrations: {0}", string.Join(", ", pending));
                }
                else
                {
                    Console.WriteLine("No pending migrations.");
                    return;
                }
            }

            if (args.Any(a => a.Equals("--migrate", StringComparison.CurrentCultureIgnoreCase)))
            {
                Console.WriteLine("Applying migrations...");
                await context.Database.MigrateAsync(cancellationToken);
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine("An error has occured: {0}", exception.Message);
            Environment.Exit(1);
        }
    }
}
