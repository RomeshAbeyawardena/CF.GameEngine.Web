using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CF.GameEngine.Infrastructure.SqlServer.Extensions;
using CF.GameEngine.Infrastructure.SqlServer;
using CF.GameEngine.MigrationTool;

Console.WriteLine("GameEngine.Web Migration tool v1.0");

var host = new HostBuilder()
    .ConfigureAppConfiguration((hostContext, config) =>
    {
        config.AddUserSecrets("3f9564c1-a8bd-4db2-b7a1-8d12479519ea");
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton(TimeProvider.System);
        services.AddSingleton<ITimeOfDayProvider, TimeOfDayProvider>();
        services.AddBackendDependencies("GameEngineDb");
    }).Build();

try
{
    var services = host.Services;
    var timeOfDayProvider = services.GetRequiredService<ITimeOfDayProvider>();
    var context = services.GetRequiredService<CFGameEngineDbContext>();

    var timeOfDay = timeOfDayProvider.GetTimeOfDay();
    Console.WriteLine("{0}", timeOfDay);

    if (await context.Database.CanConnectAsync())
    {
        Console.WriteLine("Database is connected.");
    }
    else
    {
        Console.WriteLine("Database is not connected, if this is a server related issue the following actions will fail");
    }

    if (args.Any(a => a.Equals("--migrate", StringComparison.CurrentCultureIgnoreCase)))
    {
        await context.Database.MigrateAsync();
    }
}
catch (Exception exception)
{
    Console.Error.WriteLine("An error has occured: {0}", exception.Message);
    Environment.Exit(1);
}