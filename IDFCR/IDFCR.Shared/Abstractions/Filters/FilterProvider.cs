using LinqKit;

namespace IDFCR.Shared.Abstractions.Filters;

public interface IFilterProvider<TFilter, TDb>
{
    ExpressionStarter<TDb> ApplyFilter(ExpressionStarter<TDb> query, TFilter filter);
}

public class FilterProvider<TFilter, TDb>(IEnumerable<IInjectableFilter<TFilter, TDb>> filters)
    where TFilter : IFilter<TFilter>, IInjectableFilter
{
    public ExpressionStarter<TDb> ApplyFilter(ExpressionStarter<TDb> query, TFilter filter)
    {
        foreach (var filterImplementation in filters.Where(f => f.CanApply(filter)))
        {
            query = filterImplementation.ApplyFilter(query, filter);
        }

        return query;
    }
}
