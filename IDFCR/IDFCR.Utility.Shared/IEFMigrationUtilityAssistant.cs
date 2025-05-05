using Microsoft.EntityFrameworkCore;

namespace IDFCR.Utility.Shared;

public interface IEFMigrationUtilityAssistant<TDbContext>
    where TDbContext : DbContext
{
    Task RunAsync(EFMigrationUtilityName utilityName, IEnumerable<string> args, CancellationToken cancellationToken);
}
