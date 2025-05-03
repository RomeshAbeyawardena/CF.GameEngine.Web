using LinqKit;
using Microsoft.EntityFrameworkCore;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Extensions;
using System.Linq.Dynamic.Core;
using IDFCR.Shared.Abstractions.Paging;

namespace IDFCR.Shared.EntityFramework;

public abstract class RepositoryBase<TDbContext, TAbstraction, TDb, T>(
    TimeProvider timeProvider,
    TDbContext context) 
    : IRepository<T>
    where T : class, IMappable<TAbstraction>, IIdentifer
    where TDb : class, IMappable<TAbstraction>, IIdentifer
    where TDbContext : DbContext
{
    private DbSet<TDb> DbSet => Context.Set<TDb>();

    protected ExpressionStarter<TDb> Builder => PredicateBuilder.New<TDb>(true);
    protected virtual Func<T, TDb> Map => x => x.Map<TDb>();
    protected virtual  Func<TDb, T> MapDto => x => x.Map<T>();

    protected virtual void OnAdd(TDb db, T source)
    {
        if (db is IAuditCreatedTimestamp createdTimestamp)
        {
            createdTimestamp.CreatedTimestampUtc = timeProvider.GetUtcNow();
        }
    }

    protected virtual void BeforeUpdate(TDb db, T source)
    {
        
    }

    protected virtual void OnUpdate(TDb db, T source)
    {
        if (db is IAuditModifiedTimestamp modifiedTimestamp)
        {
            modifiedTimestamp.ModifiedTimestampUtc = timeProvider.GetUtcNow();
        }
    }

    protected virtual void OnCommit(int affectedRows)
    {

    }

    protected virtual bool IsHandled(Exception exception)
    {
        return false;
    }

    protected IQueryable<TEntity> Set<TEntity>(IFilter filter)
        where TEntity : class
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if(filter.NoTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }

    protected TDbContext Context => context;

    protected async Task<IUnitPagedResult<T>> GetPagedAsync(IPagedQuery pagedQuery, IEntityOrder entityOrder, IQueryable<TDb> source, CancellationToken cancellationToken)
    {
        var conventional = pagedQuery.ToConventional();
        var query = source.Take(conventional.Take ?? 10)
            .Skip(conventional.Skip ?? 0);

        var order = entityOrder.ToString();
        if (!string.IsNullOrWhiteSpace(order))
        {
            query = query.OrderBy(order);
        }

        var result = await query.ToListAsync(cancellationToken);
        return new UnitPagedResult<T>(result.Select(MapDto), await source.CountAsync(cancellationToken), pagedQuery, UnitAction.Get);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var affectedRows = await context.SaveChangesAsync(cancellationToken);
        OnCommit(affectedRows);
        return affectedRows;
    }

    public async Task<IUnitResult<Guid>> UpsertAsync(T value, CancellationToken cancellationToken)
    {
        UnitAction unitAction;
        try
        {
            var dbValue = Map(value);
            if (dbValue.Id == Guid.Empty)
            {
                OnAdd(dbValue, value);
                await DbSet.AddAsync(dbValue, cancellationToken);
                unitAction = UnitAction.Add;
            }
            else
            {
                var foundEntry = await DbSet.FindAsync([dbValue.Id], cancellationToken);

                if (foundEntry == null)
                {
                    return new UnitResult(new EntityNotFoundException(typeof(T), dbValue.Id))
                        .As<Guid>();
                }
                BeforeUpdate(foundEntry, value);
                foundEntry.Apply(dbValue);
                OnUpdate(foundEntry, value);
                unitAction = UnitAction.Update;
            }

            return new UnitResult<Guid>(dbValue.Id, unitAction);
        }
        catch (Exception exception)
        {
            if (IsHandled(exception))
            {
                return new UnitResult(exception).As<Guid>();
            }
            //alert the consumer that the exception was not expected
            return new UnitResult(exception).AddMeta("unexpected", "true").As<Guid>();
        }
    }

    public async Task<T?> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
    {
        var result = await DbSet.FindAsync(keyValues, cancellationToken);
        if (result == null)
        {
            return default;
        }

        return MapDto(result);
    }

    public async Task<IUnitResult> DeleteAsync(object[] keyValues, CancellationToken cancellationToken)
    {
        var entity = await DbSet.FindAsync([keyValues], cancellationToken);
        if (entity == null)
        {
            return new UnitResult(new EntityNotFoundException(typeof(T), keyValues));
        }

        DbSet.Remove(entity);
        return new UnitResult(null, UnitAction.Delete, true);
    }
}
