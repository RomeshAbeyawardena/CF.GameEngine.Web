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
        config.AddUserSecrets("9315e900-337b-4d8c-9599-6b6a66b9af5e");
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

    if (args.Any(a => a.Equals("--verify-connection")))
    {
        if (await context.Database.CanConnectAsync())
        {
            Console.WriteLine("Database is connected.");
        }
        else
        {
            Console.WriteLine("Database is not connected, if this is a server related issue the following actions will fail{0}\t* Migrations{0}\t* Seeding", Environment.NewLine);
        }
    }

    if(args.Any(a => a.Equals("--list", StringComparison.CurrentCultureIgnoreCase)))
    {
        var pending = await context.Database.GetPendingMigrationsAsync();

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
        await context.Database.MigrateAsync();
    }
}
catch (Exception exception)
{
    Console.Error.WriteLine("An error has occured: {0}", exception.Message);
    Environment.Exit(1);
}