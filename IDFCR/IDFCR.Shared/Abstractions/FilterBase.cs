using LinqKit;

namespace IDFCR.Shared.Abstractions;

public abstract class FilterBase<TFilter, TDb> : MappableBase<TFilter>, IFilter<TFilter, TDb>
    where TFilter : IFilter<TFilter>
{
    /// <summary>
    /// Gets an instance of a NotSupportedException with a message indicating that mapping is not supported, 
    /// if this is intended to be the lowest level filter available to your application.
    /// </summary>
    protected NotSupportedException MappingNotSupportedException => new("This is a low-level filter, use the constructor parameter to map it as the parameters are read-only in this state");
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
