using Microsoft.AspNetCore.Routing;
using System.Linq.Expressions;

namespace IDFCR.Shared.Http.Links;

/// <summary>
/// This could use the asp.net LinkGenerator to resolve paths in an implementation
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ILinkGenerator<T>
{
    ILinkCollection GenerateLinks(T value);
}

internal class LinkGenerator<T>(LinkGenerator linkGenerator, ILinkKeyDirective linkKeyDirective,
    IReadOnlyDictionary<Expression<Func<T, object>>, ILinkReference<T>> links) : ILinkGenerator<T>
{
    private static readonly LinkExpressionVisitor linkExpressionVisitor = new();
    private static string? GetMemberName(Expression<Func<T, object>> expression)
    {
        linkExpressionVisitor.Visit(expression);
        return linkExpressionVisitor.MemberName;
    }

    private static Dictionary<string, ILink<T>> ResolveLinks(IReadOnlyDictionary<Expression<Func<T, object>>, ILinkReference<T>> links)
    {
        var dictionary = new Dictionary<string, ILink<T>>();
        
        foreach(var (key, value) in links)
        {
            var memberName = GetMemberName(key);
            if (!string.IsNullOrWhiteSpace(memberName))
            {
                var relKey = value.Rel;
                if (string.IsNullOrWhiteSpace(relKey))
                {
                    relKey = memberName;
                }

                var link = new Link<T>(value.Href, value.Method, value.Type, value.ValueExpressions, value.RouteName);

                dictionary.Add(relKey, link);
            }
        }
        return dictionary;
    }

#pragma warning disable CA1859 //Link is not exposed outside of this class
    private ILink ProduceLink(ILink<T> link, T value)
    {
        var definitions = link.ValueExpressions.ToDictionary(x => GetMemberName(x) ?? throw new NullReferenceException(),  x => x.Compile()(value));

        var href = link.Href;
        
        if (!string.IsNullOrWhiteSpace(href))
        {
            foreach(var (k,v) in definitions)
            {
                href = href.Replace($"{{{k}}}", v?.ToString(), StringComparison.InvariantCultureIgnoreCase);
            }
        }
        else
        {
            var routeValues = new RouteValueDictionary();
            if (link.ExpressionResolver != null)
            {

            }

            foreach (var (k, v) in definitions)
            {
                if (link.ExpressionResolver != null)
                {
                    routeValues.Add(k, v);
                }
            }
            href = linkGenerator.GetPathByName(link.RouteName ?? throw new NullReferenceException(), routeValues);
        }

        return new Link(href ?? throw new NullReferenceException(), link.Method, link.Type);
    }
#pragma warning restore CA1859
    private readonly Dictionary<string, ILink<T>> links = ResolveLinks(links);

    public ILinkCollection GenerateLinks(T value)
    {
        return new LinkCollection(links.ToDictionary(k => linkKeyDirective.SimplifyRel(k.Key).ToLower(), k => ProduceLink(k.Value, value)));
    }
}
