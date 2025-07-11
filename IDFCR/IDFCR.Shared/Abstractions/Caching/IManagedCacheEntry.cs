namespace IDFCR.Shared.Abstractions.Caching;

public interface IManagedCacheEntry
{
    object? Value { get; }
    TimeSpan? MaximumLifespan { get; }
    DateTimeOffset CreatedTimestampUtc { get; }
    DateTimeOffset? ModifiedTimestampUtc { get; }
    bool IsExpired(TimeProvider timeProvider);
}

public interface IManagedCacheEntry<T> : IManagedCacheEntry
{
    new T Value { get; }
}
