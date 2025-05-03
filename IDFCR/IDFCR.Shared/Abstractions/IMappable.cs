namespace IDFCR.Shared.Abstractions;

public interface IMappable<T>
{
    void Map(T source);
    TResult Map<TResult>(Func<TResult>? instanceFactory = null) where TResult : IMappable<T>;
}
