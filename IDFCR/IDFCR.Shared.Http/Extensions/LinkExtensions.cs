using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Http.Links;

namespace IDFCR.Shared.Http.Extensions;

public static class LinkExtensions
{
    public static Links.ILink<T> ToMetaLink<T>(Shared.Abstractions.ILink<T> link, string method)
        where T : IIdentifer, INamed
    {
        return new Link<T>(typeof(T).Name, method, typeof(T).Name, [x => x.Id]);
    }
}
