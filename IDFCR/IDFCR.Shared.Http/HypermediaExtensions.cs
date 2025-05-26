using IDFCR.Shared.Http.Links;

namespace IDFCR.Shared.Http;

public static class HypermediaExtensions
{
    public static IHypermedia<T> AddLink<T>(this Hypermedia<T> hypermedia, string rel, ILink link)
    {
        //hypermedia.Links[rel] = link;
        return hypermedia;
    }

    public static IHypermedia<T> AddMeta<T>(this Hypermedia<T> hypermedia, string key, object? value)
    {
        //hypermedia.Meta[key] = value;
        return hypermedia;
    }
}