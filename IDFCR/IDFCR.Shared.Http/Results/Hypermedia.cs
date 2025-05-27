using IDFCR.Shared.Http.Extensions;

namespace IDFCR.Shared.Http.Results;

public sealed record Hypermedia<T> : HypermediaBase, IHypermedia<T>
{
    public Hypermedia(T? item)
    {
        Value = item;

        if (item is null)
        {
            return;
        }

        var dictionary = item.AsDictionary();

        foreach(var kvp in dictionary)
        {
            ObjectDictionary[kvp.Key] = kvp.Value;
        }
    }

    public T? Value { get; }
}
