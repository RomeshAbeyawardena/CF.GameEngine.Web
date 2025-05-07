using System.Collections.Concurrent;

namespace IDFCR.Shared.Builders;

public interface IDictionaryBuilder<TKey, TValue>
    where TKey : notnull
{
    IDictionaryBuilder<TKey, TValue> AddOrUpdate(TKey key, TValue value);
    IDictionaryBuilder<TKey, TValue> AddOrUpdate(TKey key, TValue value, Func<TValue, TValue> updateFunc);
    Dictionary<TKey, TValue> Build();
}

public class DictionaryBuilder<TKey, TValue> : IDictionaryBuilder<TKey, TValue>
    where TKey : notnull
{
    private readonly ConcurrentDictionary<TKey, TValue> _dictionary = [];
    public IDictionaryBuilder<TKey, TValue> AddOrUpdate(TKey key, TValue value)
    {
        _dictionary.AddOrUpdate(key, value, (k, v) => value);
        return this;
    }

    public IDictionaryBuilder<TKey, TValue> AddOrUpdate(TKey key, TValue value, Func<TValue, TValue> updateFunc)
    {
        _dictionary.AddOrUpdate(key, value, (k, v) => updateFunc(v));
        return this;
    }

    public Dictionary<TKey, TValue> Build()
    {
        return _dictionary.ToDictionary();
    }
}
