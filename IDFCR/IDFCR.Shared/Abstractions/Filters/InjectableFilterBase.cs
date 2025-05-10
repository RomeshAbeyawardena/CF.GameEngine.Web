using LinqKit;

namespace IDFCR.Shared.Abstractions.Filters;

public abstract class InjectableFilterBase<TFilter, TDb> : FilterBase<TFilter, TDb>, IInjectableFilter<TFilter, TDb>
    where TFilter : IFilter<TFilter>, IInjectableFilter
{
    public virtual bool ShouldApply(TFilter filter)
    {
        return true;
    }

    public override ExpressionStarter<TDb> ApplyFilter(ExpressionStarter<TDb> query)
    {
        throw new NotSupportedException("Injectable filters are unaware of the base filters during instantiation");
    }
}
