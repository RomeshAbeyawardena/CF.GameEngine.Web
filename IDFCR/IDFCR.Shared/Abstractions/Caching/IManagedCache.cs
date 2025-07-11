namespace IDFCR.Shared.Abstractions.Caching;

public interface IManagedCacheEntry
{
    object? Value { get; }
    TimeSpan? MaximumLifespan { get; }
    DateTimeOffset CreatedTimestampUtc { get; }
    DateTimeOffset? ModifiedTimestampUtc { get; }
    bool IsExpired(TimeProvider timeProvider);
}

public interface IManagedCache<TKey, T>
{
    bool Contains(TKey key);
    bool TryGetValue(TKey key, out T? value);
    Task<T> GetOrAddAsync(TKey key, Func<TKey, CancellationToken, Task<T>> addEntry, CancellationToken? cancellationToken = null, 
        Action<IManagedCacheEntry<T>>? configureEntry = null);
}
