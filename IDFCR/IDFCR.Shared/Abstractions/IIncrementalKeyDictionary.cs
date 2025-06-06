using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace IDFCR.Shared.Abstractions;

public interface IIncrementalKeyDictionary<TValue> : IDictionary<string, TValue>
{
}

public class IncrementalKeyDictionary<TValue> : IIncrementalKeyDictionary<TValue>
{
    private readonly ConcurrentDictionary<string, TValue> internalDictionary = [];

    public TValue this[string key] { get => internalDictionary[key]; set => internalDictionary[key] = value; }

    public ICollection<string> Keys => internalDictionary.Keys;
    public ICollection<TValue> Values => internalDictionary.Values;
    public int Count => internalDictionary.Count;
    public bool IsReadOnly => false;

    public void Add(string key, TValue value)
    {
        if (ContainsKey(key))
        {
            var count = internalDictionary.Keys.Where(x => x.StartsWith(key)).Count();

            var newKey = $"{key}_{count}";
            if (!internalDictionary.TryAdd(newKey, value))
            {
                internalDictionary[newKey] = value;
            }
        }
        else
        {
            if(!internalDictionary.TryAdd(key, value))
            {
                internalDictionary[key] = value;
            }
        }
    }

    public void Add(KeyValuePair<string, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Clear()
    {
        internalDictionary.Clear();
    }

    public bool Contains(KeyValuePair<string, TValue> item)
    {
        return internalDictionary.Contains(item);
    }

    public bool ContainsKey(string key)
    {
        return internalDictionary.Keys.Any(x => x.StartsWith(key));
    }

    public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
    {

    }

    public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
    {
        return internalDictionary.GetEnumerator();
    }

    public bool Remove(string key)
    {
        return internalDictionary.TryRemove(key, out _);
    }

    public bool Remove(KeyValuePair<string, TValue> item)
    {
        return internalDictionary.TryRemove(item.Key, out _);
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value)
    {
        return internalDictionary.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return internalDictionary.GetEnumerator();
    }
}
