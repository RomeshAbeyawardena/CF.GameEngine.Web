namespace IDFCR.Shared.Abstractions.Filters;

public interface IInjectableFilter { }
public interface IInjectableFilter<TFilter, TDb> : IFilter<TFilter, TDb>, IInjectableFilter
    where TFilter : IFilter<TFilter>
{
    bool CanApply(TFilter filter);
}
