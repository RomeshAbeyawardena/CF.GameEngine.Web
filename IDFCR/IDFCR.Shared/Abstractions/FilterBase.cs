using LinqKit;

namespace IDFCR.Shared.Abstractions;

public abstract class FilterBase<TFilter, TDb> : MappableBase<TFilter>, IFilter<TFilter, TDb>
    where TFilter : IFilter<TFilter>
{
    protected FilterBase(TFilter? targetFilter = default)
    {
        if (targetFilter is not null)
        {
            Map(targetFilter);
        }
    }

    public bool NoTracking { get; set; }

    public virtual ExpressionStarter<TDb> ApplyFilter(ExpressionStarter<TDb> query, TFilter filter)
    {
        return query;
    }
}
