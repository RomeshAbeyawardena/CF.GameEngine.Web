using IDFCR.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IDFCR.Utility.Shared;

public static class EFMigrationUtility
{
    public static IEnumerable<string> Operations => ["list", "migrate", "verify-connection"];
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
    private readonly Dictionary<EFMigrationUtilityExtensionName, 
        Func<ILogger<IEFMigrationUtilityAssistant<TDbContext>>, TDbContext, IEnumerable<string>, IServiceProvider, CancellationToken, Task<MigrationResult>>> _extensions = [];

    internal IReadOnlyDictionary<EFMigrationUtilityExtensionName, 
        Func<ILogger<IEFMigrationUtilityAssistant<TDbContext>>, TDbContext, IEnumerable<string>, IServiceProvider, CancellationToken, Task<MigrationResult>>> Extensions => _extensions;

    public IHost? Host { get; private set; }

    public IEnumerable<MigrationResult> Results { get; private set; } = [];

    public async Task RunMigrationAssistant(IHost host, CancellationToken cancellationToken)
    {
        var migrationAssitant = host.Services.GetRequiredService<IEFMigrationUtilityAssistant<TDbContext>>() as EFMigrationUtilityAssistant<TDbContext> 
            ?? throw new InvalidCastException();

        migrationAssitant.Instance = this;
        Results = (await migrationAssitant.RunAsync(utilityName, args, cancellationToken)).GetResultOrDefault() ?? [];
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

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }

    public IEFMigrationUtility<TDbContext> Extend(string name, string? description,
        Func<ILogger<IEFMigrationUtilityAssistant<TDbContext>>, TDbContext, IEnumerable<string>, CancellationToken, Task<MigrationResult>> extension)
    {
        return Extend(name, description, (l, ct, s, a, c) => extension(l, ct, s, c));
        
    }

    public IEFMigrationUtility<TDbContext> Extend(string name, string? description, 
        Func<ILogger<IEFMigrationUtilityAssistant<TDbContext>>, TDbContext, IEnumerable<string>, IServiceProvider, CancellationToken, Task<MigrationResult>> extension)
    {
        if (!_extensions.TryAdd(new(name, description), extension))
        {
            throw new InvalidOperationException($"An extension with the name '{name}' is already registered.");
        }
        return this;
    }
}
