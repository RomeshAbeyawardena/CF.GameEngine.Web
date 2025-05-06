using System.Linq.Expressions;

namespace IDFCR.Shared.Http.Links;

internal record LinkReference<T>(string? Href, string Method, string Type, IEnumerable<Expression<Func<T, object>>> ValueExpressions,
    string? Rel = null, string? RouteName = null) : ILinkReference<T>;

internal record Link<T>(string? Href, string Method, string Type, IEnumerable<Expression<Func<T, object>>> ValueExpressions, 
    string? RouteName = null)
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