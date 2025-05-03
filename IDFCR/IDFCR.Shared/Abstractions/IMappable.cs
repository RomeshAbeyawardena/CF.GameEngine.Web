namespace IDFCR.Shared.Abstractions;

public interface ISingularMappable<T>
{
    void Map(T source);
}

public interface IMappable<T> : ISingularMappable<T>
{
    TResult Map<TResult>(Func<TResult>? instanceFactory = null) where TResult : IMappable<T>;
}
