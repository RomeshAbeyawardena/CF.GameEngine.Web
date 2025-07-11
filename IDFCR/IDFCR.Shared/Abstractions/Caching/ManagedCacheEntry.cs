namespace IDFCR.Shared.Abstractions.Caching;

public record ManagedCacheEntry<T>(T Value) : IManagedCacheEntry<T>
{
    object? IManagedCacheEntry.Value => Value;
    public TimeSpan? MaximumLifespan { get; init; }
    public DateTimeOffset CreatedTimestampUtc { get; private set; }
    public DateTimeOffset? ModifiedTimestampUtc { get; private set; }

    public bool IsExpired(TimeProvider timeProvider) => MaximumLifespan.HasValue &&
        CreatedTimestampUtc + MaximumLifespan < timeProvider.GetUtcNow();

    internal void MarkAsCreated(TimeProvider timeProvider) {
        if (CreatedTimestampUtc == default)
        {
            CreatedTimestampUtc = timeProvider.GetUtcNow();
        }
    }

    internal void MarkAsModified(TimeProvider timeProvider) => ModifiedTimestampUtc = timeProvider.GetUtcNow();
}
