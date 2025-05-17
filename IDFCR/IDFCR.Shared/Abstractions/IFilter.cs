using LinqKit;

namespace IDFCR.Shared.Abstractions;

public interface IFilter
{
    //Shared filter logic here that can be shared with the domain layer here
    bool NoTracking { get; }
}

public interface IFilter<TFilter> : IFilter
{
    //Generic Shared filter logic here that can be shared with the domain layer here
    
}

public interface IFilter<TFilter, TDb> : IMappable<TFilter>, IFilter
    where TFilter : IFilter<TFilter>
{
    TFilter Filter { set; }
    ExpressionStarter<TDb> ApplyFilter(ExpressionStarter<TDb> query, TFilter filter);
    Task<ExpressionStarter<TDb>> ApplyFilterAsync(ExpressionStarter<TDb> query, CancellationToken cancellationToken);
}
