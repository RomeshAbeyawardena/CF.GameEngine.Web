namespace IDFCR.Shared.Abstractions.Records;

public abstract record MappableBase<T> : IMappable<T>
{
    protected abstract T Source { get; }
    public abstract void Map(T source);
    public TResult Map<TResult>(Func<TResult>? instanceFactory = null) where TResult : IMappable<T>
    {
        var result = instanceFactory == null 
            ? Activator.CreateInstance<TResult>() 
            : instanceFactory();
        result.Map(Source);
        return result;
    }
}
