using LinqKit;

namespace IDFCR.Shared.Abstractions.Filters;

public class FilterProvider<TFilter, TDb>(IEnumerable<IInjectableFilter<TFilter, TDb>> filters)
    where TFilter : IFilter<TFilter>
{
    public ExpressionStarter<TDb> ApplyFilter(ExpressionStarter<TDb> query, TFilter filter)
    {
        foreach (var filterImplementation in filters.Where(f => f.ShouldApply(filter)))
        {
            query = filterImplementation.ApplyFilter(query, filter);
        }

        return query;
    }
}
