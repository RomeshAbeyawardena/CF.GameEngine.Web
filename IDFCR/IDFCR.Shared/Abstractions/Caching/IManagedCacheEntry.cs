namespace IDFCR.Shared.Abstractions.Caching;

public interface IManagedCacheEntry<T> : IManagedCacheEntry
{
    new T Value { get; }
}
