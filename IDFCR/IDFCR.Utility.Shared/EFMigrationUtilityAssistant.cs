using IDFCR.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.RegularExpressions;

namespace IDFCR.Utility.Shared;

internal class EFMigrationUtilityAssistant<TDbContext>(IServiceProvider serviceProvider, ILogger<EFMigrationUtilityAssistant<TDbContext>> logger,
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

            const int FixedLength = 40;
            if (args.Any(a => a.Equals("--help", StringComparison.OrdinalIgnoreCase)))
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Available options:");

                var builtinKey = "verify-connection".FixedLength(FixedLength);
                stringBuilder.AppendLine($"\t--{builtinKey}\tCheck if DB is reachable");
                builtinKey = "list".FixedLength(FixedLength);
                stringBuilder.AppendLine($"\t--{builtinKey}\tList pending migrations");
                builtinKey = "migrate".FixedLength(FixedLength);
                stringBuilder.AppendLine($"\t--{builtinKey}\tApply migrations");
                builtinKey = "suppress-duplicate-directive-warning".FixedLength(FixedLength);
                stringBuilder.AppendLine($"\t--{builtinKey}\tSuppress warnings about duplicate directives exisiting in both primary and extension contexts");

                logger.LogInformation("{stringBuilder}", stringBuilder.ToString());
                stringBuilder.Clear();

                if(Instance?.Extensions.Count == 0)
                {
                    logger.LogInformation("No extensions available.");
                    return;
                }

                stringBuilder.AppendLine("Available extensions:");
                foreach (var key in Instance?.Extensions.Keys ?? [])
                {
                    if (!args.Any(a => a.Equals("--suppress-duplicate-directive-warning", StringComparison.InvariantCultureIgnoreCase)) 
                        && EFMigrationUtility.Operations.Contains(key.Name))
                    {
                        logger.LogWarning("The directive {key} will be run twice, once as a primary directive and again as an extended directive, be wary of side effects!", key);
                    }

                    builtinKey = key.Name.FixedLength(FixedLength);
                    stringBuilder.AppendLine($"\t--{builtinKey}\tRun extension: {key.Description}");
                }

                logger.LogInformation("{stringBuilder}", stringBuilder.ToString());
                return;
            }

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
                    //wildcard match
                    Func<string, bool> predicate = a => a.Equals($"{key.Name}", StringComparison.CurrentCultureIgnoreCase);
                    if (key.Name.Contains('*'))
                    {
                        var searchValue = key.Name.Replace("*", "[:|A-z|0-9|-]{0,}");

                        var regex = new Regex($"^{searchValue}");

                        predicate = regex.IsMatch;
                    }

                    if (args.Any(predicate))
                    {
                        logger.LogInformation("Running extension: {Name}", key.Name);

                        await extension(logger, context, args, serviceProvider, cancellationToken);
                    }
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
