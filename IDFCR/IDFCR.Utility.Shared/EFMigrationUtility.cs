using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IDFCR.Utility.Shared;

public record EFMigrationUtilityName(string Name, string Version);

public class EFMigrationUtility<TDbContext>(EFMigrationUtilityName utilityName, IEnumerable<string> args, string userSecretId, Action<HostBuilderContext, IServiceCollection> configureServices)
    where TDbContext : DbContext
{
    private Task RunMigrationAssistant(IHost host, CancellationToken cancellationToken)
    {
        var migrationAssitant = host.Services.GetRequiredService<IEFMigrationUtilityAssistant<TDbContext>>();

        return migrationAssitant.RunAsync(utilityName, args, cancellationToken);
    }

    public async Task InitialiseAsync(CancellationToken cancellationToken = default)
    {
        using var host = new HostBuilder()
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddUserSecrets(userSecretId);
            })
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(TimeProvider.System);
                services.AddSingleton<ITimeOfDayProvider, TimeOfDayProvider>();
                services.AddSingleton<IEFMigrationUtilityAssistant<TDbContext>, EFMigrationUtilityAssistant<TDbContext>>();
                configureServices(hostContext, services);
            }).Build();

       await RunMigrationAssistant(host, cancellationToken);
    }
}
