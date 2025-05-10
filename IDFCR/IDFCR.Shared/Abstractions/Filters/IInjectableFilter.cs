using LinqKit;

namespace IDFCR.Shared.Abstractions.Filters;

public interface IInjectableFilter<TFilter, TDb>
    where TFilter : IFilter<TFilter>
{
    bool ShouldApply(TFilter filter);
    ExpressionStarter<TDb> ApplyFilter(ExpressionStarter<TDb> query, TFilter filter);
}
