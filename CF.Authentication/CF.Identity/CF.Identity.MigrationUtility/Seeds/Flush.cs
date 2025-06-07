using CF.Identity.Infrastructure.SqlServer;
using IDFCR.Utility.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CF.Identity.MigrationUtility.Seeds;

public static class Flush
{
    public static async Task<MigrationResult> FlushSeedAsync(ILogger logger, CFIdentityDbContext context, IEnumerable<string> args,
        IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        if (!args.Any(a => a.Equals("--force", StringComparison.OrdinalIgnoreCase)))
        {
            logger.LogWarning("You must pass the --force flag to flush seed data. This is a destructive operation and cannot be undone.");
            return new MigrationResult(nameof(FlushSeedAsync), MigrationStatus.Failed, "Operation cancelled: --force flag not provided.");
        }

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            logger.LogInformation("Flushing seed data from the database using low-level calls.");
            logger.LogWarning("🔥 FLUSHING DEV INSTANCE: This action is irreversible, unholy, and probably caused by self-inflicted pain.");

            await context.Database.ExecuteSqlAsync(
                $@"DELETE FROM [AccessToken]
               DELETE FROM [UserScope]
               DELETE FROM [Scope]
               DELETE FROM [User]
               DELETE FROM [Client]", cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return new MigrationResult(nameof(FlushSeedAsync), MigrationStatus.Completed, "Seed data flushed successfully.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Failed to flush seed data. This may be due to foreign key constraints or other relational bindings.");
            await transaction.RollbackAsync(cancellationToken);
            return new MigrationResult(nameof(FlushSeedAsync), MigrationStatus.Failed, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while flushing seed data.");
            await transaction.RollbackAsync(cancellationToken);
            return new MigrationResult(nameof(FlushSeedAsync), MigrationStatus.Failed, ex.Message);
        }
    }
}
