using System.Linq.Expressions;

namespace IDFCR.Shared.Http.Links;

internal record LinkReference<T>(string? Href, string Method, string Type, IEnumerable<Expression<Func<T, object>>> ValueExpressions,
    string? Rel = null, string? RouteName = null, IReadOnlyDictionary<Expression<Func<T, object>>, string>? ExpressionResolver = null) : ILinkReference<T>;

internal record Link<T>(string? Href, string Method, string Type, IEnumerable<Expression<Func<T, object>>> ValueExpressions, 
    string? RouteName = null, IReadOnlyDictionary<Expression<Func<T, object>>, string>? ExpressionResolver = null)
    : ILink<T>;

internal record Link(string Href, string Method, string Type) : ILink
{
    public static ILink Empty = new Link(string.Empty, string.Empty, string.Empty);
}