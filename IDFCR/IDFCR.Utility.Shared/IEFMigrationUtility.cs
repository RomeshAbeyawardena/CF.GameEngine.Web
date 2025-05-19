using IDFCR.Shared.Mediatr;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IDFCR.Utility.Shared;

public interface IEFMigrationUtility<TDbContext> : IDisposable, IAsyncDisposable
    where TDbContext : DbContext
{
    IEnumerable<MigrationResult> Results { get; }
    IHost? Host { get; }
    IEFMigrationUtility<TDbContext> Extend(string name, string? description,
        Func<ILogger<IEFMigrationUtilityAssistant<TDbContext>>, TDbContext, IEnumerable<string>,CancellationToken,Task<MigrationResult>> extension);

    IEFMigrationUtility<TDbContext> Extend(string name, string? description,
        Func<ILogger<IEFMigrationUtilityAssistant<TDbContext>>, TDbContext, IEnumerable<string>, IServiceProvider, CancellationToken, Task<MigrationResult>> extension);

    Task RunMigrationAssistant(IHost host, CancellationToken cancellationToken);
    Task InitialiseAsync(bool runMigrationAssistance = true, CancellationToken cancellationToken = default);
}
