using IDFCR.Shared.Abstractions.Results;
using Microsoft.EntityFrameworkCore;

namespace IDFCR.Utility.Shared;

public enum MigrationStatus
{
    Unknown = 0,
    InProgress = 1,
    Completed = 2,
    CompletedWithErrors = 3,
    Failed = 4
}
public record MigrationResult(string Key, MigrationStatus Status, string? Message = null, Exception? Exception = null);

public interface IEFMigrationUtilityAssistant<TDbContext>
    where TDbContext : DbContext
{
    Task<IUnitResultCollection<MigrationResult>> RunAsync(EFMigrationUtilityName utilityName, IEnumerable<string> args, CancellationToken cancellationToken);
}
