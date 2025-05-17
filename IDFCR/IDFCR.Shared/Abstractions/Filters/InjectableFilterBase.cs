namespace IDFCR.Shared.Abstractions.Filters;

public abstract class InjectableFilterBase<TFilter, TDb> : FilterBase<TFilter, TDb>, IInjectableFilter<TFilter, TDb>
    where TFilter : IFilter<TFilter>, IInjectableFilter
{
    protected override TFilter Source => _filter ?? throw new NullReferenceException();

    public virtual bool CanApply(TFilter filter)
    {
        return true;
    }
}
