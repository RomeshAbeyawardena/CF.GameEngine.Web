using LinqKit;

namespace IDFCR.Shared.Abstractions.Filters;

public abstract class FilterBase<TFilter, TDb> : MappableBase<TFilter>, IFilter<TFilter, TDb>
    where TFilter : IFilter<TFilter>
{
    protected TFilter? _filter;
    public TFilter Filter { set => _filter = value; }
    /// <summary>
    /// Gets an instance of a NotSupportedException with a message indicating that mapping is not supported, 
    /// if this is intended to be the lowest level filter available to your application.
    /// </summary>
    protected NotSupportedException MappingNotSupportedException => new(
        "This is a low-level filter, use the constructor parameter to map it as the parameters are read-only in this state");
    protected FilterBase(TFilter? targetFilter = default)
    {
        if (targetFilter is not null)
        {
            Map(targetFilter);
        }

        _filter = targetFilter;
    }

    public bool NoTracking { get; set; }

    public virtual Task<ExpressionStarter<TDb>> ApplyFilterAsync(ExpressionStarter<TDb> query, CancellationToken cancellationToken)
    {
        return Task.FromResult(ApplyFilter(query));
    }

    public virtual ExpressionStarter<TDb> ApplyFilter(ExpressionStarter<TDb> query)
    {
        return ApplyFilter(query, _filter ?? throw new NullReferenceException("Filter not supplied"));
    }

    public virtual Task<ExpressionStarter<TDb>> ApplyFilterAsync(ExpressionStarter<TDb> query, TFilter filter, CancellationToken cancellationToken)
    {
        return Task.FromResult(ApplyFilter(query, filter));
    }

    public abstract ExpressionStarter<TDb> ApplyFilter(ExpressionStarter<TDb> query, TFilter filter);
}
