using System.Linq.Expressions;

namespace IDFCR.Shared.Http.Links;

internal record LinkReference(string Href, string Method, string Type, string? Rel = null) : ILinkReference;

internal record Link<T>(string Href, string Method, string Type, Expression<Func<T, object>> ValueExpression)
    : LinkPlaceholderBag, ILink<T>
{
    public ILink<T> AddOrUpdateBag(string key, string value)
    {
        if(!PlaceholderBag.TryAdd(key, value))
        {
            PlaceholderBag[key] = value;
        };

        return this;
    }
}

internal record Link(string Href, string Method, string Type) : ILink;