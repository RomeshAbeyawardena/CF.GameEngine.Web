using IDFCR.Shared.Http.Extensions;
using System.Dynamic;

namespace IDFCR.Shared.Http;

public record Hypermedia<T> : HypermediaBase, IHypermedia<T>
{
    public Hypermedia(T? item)
    {
        if(item is null)
        {
            return;
        }

        var dictionary = item.ToDictionary();

        foreach(var kvp in dictionary)
        {
            ObjectDictionary[kvp.Key] = kvp.Value;
        }
    }
}
