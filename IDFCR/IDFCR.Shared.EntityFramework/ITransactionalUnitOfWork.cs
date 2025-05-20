using IDFCR.Shared.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IDFCR.Shared.EntityFramework;

public interface ITransactionalUnitOfWork : IUnitOfWork, IDisposable
{
    Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken);
    Task RollbackAsync(CancellationToken cancellationToken);
}

public class TransactionalUnitOfWork<TDbContext>(TDbContext dbContext) : ITransactionalUnitOfWork
    where TDbContext : DbContext
{
    private IDbContextTransaction? transaction;

    protected TDbContext Context => dbContext;

    public async Task<IDbContextTransaction?> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        EnsureDbContext();

        if (transaction is not null)
        {
            throw new InvalidOperationException("A transaction is already in progress.");
        }

        return transaction = await dbContext!.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken)
    {
        EnsureDbContext();

        if (transaction is null)
        {
            throw new InvalidOperationException("No active transaction to commit.");
        }

        await transaction.CommitAsync(cancellationToken);
        await transaction.DisposeAsync();
        transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        EnsureDbContext();

        if (transaction is not null)
        {
            await transaction.RollbackAsync(cancellationToken);
            await transaction.DisposeAsync();
            transaction = null;
        }
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        EnsureDbContext();
        return await dbContext!.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        transaction?.Dispose();
        transaction = null;
        GC.SuppressFinalize(this);
    }

    private void EnsureDbContext()
    {
        if (dbContext is null)
        {
            throw new InvalidOperationException("DbContext has not been assigned.");
        }
    }
}

