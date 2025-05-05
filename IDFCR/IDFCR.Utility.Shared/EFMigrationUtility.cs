using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IDFCR.Utility.Shared;

public static class EFMigrationUtility
{
    public static IEFMigrationUtility<TDbContext> MigrationUtility<TDbContext>(EFMigrationUtilityName utilityName, IEnumerable<string> args,
    string userSecretId, Action<HostBuilderContext, IServiceCollection> configureServices)
        where TDbContext : DbContext
    {
        return new EFMigrationUtility<TDbContext>(utilityName, args, userSecretId, configureServices);
    }
}

internal class EFMigrationUtility<TDbContext>(EFMigrationUtilityName utilityName, IEnumerable<string> args, 
    string userSecretId, Action<HostBuilderContext, IServiceCollection> configureServices) : IEFMigrationUtility<TDbContext>
    where TDbContext : DbContext
{
    public IHost? Host { get; private set; }

    public Task RunMigrationAssistant(IHost host, CancellationToken cancellationToken)
    {
        var migrationAssitant = host.Services.GetRequiredService<IEFMigrationUtilityAssistant<TDbContext>>();

        return migrationAssitant.RunAsync(utilityName, args, cancellationToken);
    }

    public async Task InitialiseAsync(bool runMigrationAssistance = true, CancellationToken cancellationToken = default)
    {
        Host = new HostBuilder()
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

        if (runMigrationAssistance)
        {
            await RunMigrationAssistant(Host, cancellationToken);
        }
    }

    public void Dispose()
    {
        Host?.Dispose();
    }
}
