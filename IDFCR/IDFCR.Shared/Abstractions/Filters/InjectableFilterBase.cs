using LinqKit;

namespace IDFCR.Shared.Abstractions.Filters;

public abstract class InjectableFilterBase<TFilter, TDb> : IInjectableFilter<TFilter, TDb>
    where TFilter : IFilter<TFilter>
{
    public bool NoTracking { get; protected set; }
    public abstract ExpressionStarter<TDb> ApplyFilter(ExpressionStarter<TDb> query, TFilter filter);

    public virtual async Task<ExpressionStarter<TDb>> ApplyFilterAsync(ExpressionStarter<TDb> query, TFilter filter, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return ApplyFilter(query, filter);
    }

    public virtual bool CanApply(TFilter filter)
    {
        return true;
    }
}
