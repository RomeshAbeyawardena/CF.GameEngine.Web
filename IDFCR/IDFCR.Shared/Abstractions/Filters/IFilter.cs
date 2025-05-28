using LinqKit;

namespace IDFCR.Shared.Abstractions.Filters;

/// <summary>
/// Represents a shared filter model
/// </summary>
public interface IFilter
{
    //Shared filter logic here that can be shared with the domain layer here
    bool NoTracking { get; }
}

/// <summary>
/// Represents a filter model of <see cref="TFilter"/>
/// </summary>
/// <typeparam name="TFilter">The type of filter</typeparam>
public interface IFilter<TFilter> : IFilter
    where TFilter : IFilter
{
    //Generic Shared filter logic here that can be shared with the domain layer here
    
}

/// <summary>
/// Represents a db entity specific filter
/// </summary>
/// <typeparam name="TFilter">The type of filter</typeparam>
/// <typeparam name="TDb">Db entity to filter</typeparam>
public interface IFilter<TFilter, TDb> : IFilter
    where TFilter : IFilter<TFilter>
{
    ExpressionStarter<TDb> ApplyFilter(ExpressionStarter<TDb> query, TFilter filter);
    Task<ExpressionStarter<TDb>> ApplyFilterAsync(ExpressionStarter<TDb> query, TFilter filter, CancellationToken cancellationToken);
}


/// <summary>
/// Represents a mappable db entity specific filter where the filter owns the data to be used for filtering
/// </summary>
/// <typeparam name="TFilter">The type of filter</typeparam>
/// <typeparam name="TDb">Db entity to filter</typeparam>
public interface IMappableFilter<TFilter, TDb> : IMappable<TFilter>, IFilter<TFilter, TDb>, IFilter
    where TFilter : IFilter<TFilter>
{
    TFilter Filter { set; }
    Task<ExpressionStarter<TDb>> ApplyFilterAsync(ExpressionStarter<TDb> query, CancellationToken cancellationToken);
}
