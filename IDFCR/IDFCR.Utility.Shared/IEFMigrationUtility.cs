using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IDFCR.Utility.Shared;

public interface IEFMigrationUtility<TDbContext> : IDisposable, IAsyncDisposable
    where TDbContext : DbContext
{
    IHost? Host { get; }
    IEFMigrationUtility<TDbContext> Extend(string name, string? description,
        Func<ILogger<IEFMigrationUtilityAssistant<TDbContext>>, TDbContext, IEnumerable<string>,CancellationToken,Task> extension);
    Task RunMigrationAssistant(IHost host, CancellationToken cancellationToken);
    Task InitialiseAsync(bool runMigrationAssistance = true, CancellationToken cancellationToken = default);
}
