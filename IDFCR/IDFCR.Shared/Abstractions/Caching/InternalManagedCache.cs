using System.Collections.Concurrent;

namespace IDFCR.Shared.Abstractions.Caching;

public class InternalManagedCache<TKey, T>(TimeProvider timeProvider, int limit = 5_000) : IManagedCache<TKey, T>
    where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, IManagedCacheEntry<T>> cache = [];

    private ConcurrentDictionary<TKey, Task<T>> pending = [];

    public bool Contains(TKey key)
    {
        bool expired = false;

        if(cache.TryGetValue(key, out var value) && !(expired = value.IsExpired(timeProvider)))
        {
            return true;
        }

        if (expired)
        {
            cache.TryRemove(key, out _);
        }

        return false;
    }

    public async Task<T> GetOrAddAsync(TKey key, Func<TKey, CancellationToken, Task<T>> addEntry, CancellationToken? cancellationToken = null, Action<IManagedCacheEntry<T>>? configureEntry = null)
    {
        if(pending.TryGetValue(key, out var result))
        {
            return await result;
        }

        var task = Task.Run(async () =>
        {
            if (cache.TryGetValue(key, out var entry))
            {
                ((ManagedCacheEntry<T>)entry).MarkAsModified(timeProvider);

                return entry.Value;
            }

            var value = await addEntry(key, cancellationToken.GetValueOrDefault(CancellationToken.None));

            if (value is not null && cache.Count + 1 > limit)
            {
                var toBeEvicted = cache.FirstOrDefault(x => x.Value.IsExpired(timeProvider));

                if (toBeEvicted.Key is null)
                {
                    toBeEvicted = cache.OrderBy(x => x.Value.ModifiedTimestampUtc ?? x.Value.CreatedTimestampUtc).FirstOrDefault();
                }

                if (toBeEvicted.Key is not null)
                {
                    cache.TryRemove(toBeEvicted.Key, out _);
                }
            }

            var cacheEntry = new ManagedCacheEntry<T>(value);
            cacheEntry.MarkAsCreated(timeProvider);
            configureEntry?.Invoke(cacheEntry);
            cache.TryAdd(key, cacheEntry);

            pending.TryRemove(key, out _);

            return value;
        });

        pending.TryAdd(key, task);

        return await task.ConfigureAwait(false);
    }

    public bool TryGetValue(TKey key, out T? value)
    {
        value = default;
        var isExpired = false;
        if(cache.TryGetValue(key, out var val) && !(isExpired = val.IsExpired(timeProvider)))
        {
            value = val.Value;

            return true;
        }

        if (isExpired)
        {
            cache.TryRemove(key, out _);
        }

        return false;
    }
}
