using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IDFCR.Utility.Shared;

internal class EFMigrationUtilityAssistant<TDbContext>(ILogger<EFMigrationUtilityAssistant<TDbContext>> logger,
        ITimeOfDayProvider timeOfDayProvider, TDbContext context) : IEFMigrationUtilityAssistant<TDbContext>
    where TDbContext : DbContext
{
    internal EFMigrationUtility<TDbContext>? Instance { get; set; }

    public async Task RunAsync(EFMigrationUtilityName utilityName, IEnumerable<string> args, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("{Name} Migration tool v{Version}", utilityName.Name, utilityName.Version);

            var timeOfDay = timeOfDayProvider.GetTimeOfDay();
            logger.LogInformation("{timeOfDay}", timeOfDay);

            if (args.Any(a => a.Equals("--verify-connection")))
            {
                if (await context.Database.CanConnectAsync(cancellationToken))
                {
                    logger.LogInformation("Database is connected.");
                }
                else
                {
                    var newLine = Environment.NewLine;
                    logger.LogInformation("Database is not connected, if this is a server related issue the following actions will fail{newLine}\t* Migrations{newLine}\t* Seeding", newLine, newLine);
                }
            }

            if (args.Any(a => a.Equals("--list", StringComparison.CurrentCultureIgnoreCase)))
            {
                var pending = await context.Database.GetPendingMigrationsAsync(cancellationToken);

                if (pending.Any())
                {
                    logger.LogInformation("Pending migrations: {pending}", string.Join(", ", pending));
                }
                else
                {
                    logger.LogInformation("No pending migrations.");
                    return;
                }
            }

            if (args.Any(a => a.Equals("--migrate", StringComparison.CurrentCultureIgnoreCase)))
            {
                logger.LogInformation("Applying migrations...");
                await context.Database.MigrateAsync(cancellationToken);
            }

            if (Instance is not null)
            {
                foreach(var (key, extension) in Instance.Extensions)
                {
                    await extension(logger, context, args, cancellationToken);
                }
            }
        }
        catch (Exception exception)
        {
            logger.LogError("An error has occured: {Message}", exception.Message);
            Environment.Exit(1);
        }
    }
}
