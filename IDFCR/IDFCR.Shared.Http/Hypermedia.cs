using IDFCR.Shared.Http.Extensions;

namespace IDFCR.Shared.Http;

public sealed record Hypermedia<T> : HypermediaBase, IHypermedia<T>
{
    public Hypermedia(T? item)
    {
        Value = item;

        if (item is null)
        {
            return;
        }

        var dictionary = item.ToDictionary();

        foreach(var kvp in dictionary)
        {
            ObjectDictionary[kvp.Key] = kvp.Value;
        }
    }

    public T? Value { get; }
}
