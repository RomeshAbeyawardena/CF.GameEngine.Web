using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace IDFCR.Utility.Shared;

public interface IEFMigrationUtility<TDbContext> : IDisposable
    where TDbContext : DbContext
{
    IHost? Host { get; }
    Task RunMigrationAssistant(IHost host, CancellationToken cancellationToken);
    Task InitialiseAsync(bool runMigrationAssistance = true, CancellationToken cancellationToken = default);
}
