namespace IDFCR.Shared.Abstractions.Caching;

public record ManagedCacheEntry<T>(T Value) : IManagedCacheEntry<T>
{
    object? IManagedCacheEntry.Value => Value;

    public TimeSpan? MaximumLifespan { get; init; }
    public DateTimeOffset CreatedTimestampUtc { get; private set; }
    private DateTimeOffset? _ModifiedTimestampUtc;
    private readonly Lock @lock = new();
    public DateTimeOffset? ModifiedTimestampUtc
    {
        get
        {
            using var _ = @lock.EnterScope();
            var value = _ModifiedTimestampUtc;
            
            return value;
        }
        private set
        {
            using var _ = @lock.EnterScope();
            _ModifiedTimestampUtc = value;
        }
    }

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
