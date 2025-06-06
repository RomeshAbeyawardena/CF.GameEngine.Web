namespace IDFCR.Shared.Abstractions;

public static class EnumerableExtensions
{
    public static IIncrementalKeyDictionary<T> ToIncrementalKeyDictionary<TValue, T>(this IEnumerable<TValue> value, Func<TValue, string> getKey, Func<TValue, T> getValue)
    {
        IncrementalKeyDictionary<T> incrementalKeyDictionary = [];

        foreach (var val in value)
        {
            var key = getKey(val);

            incrementalKeyDictionary.Add(key, getValue(val));
        }

        return incrementalKeyDictionary;
    }

    public static IIncrementalKeyDictionary<TValue> ToIncrementalKeyDictionary<TValue>(this IEnumerable<TValue> value, Func<TValue, string> getKey)
    {
        return value.ToIncrementalKeyDictionary(getKey, x => x);
    }
}
