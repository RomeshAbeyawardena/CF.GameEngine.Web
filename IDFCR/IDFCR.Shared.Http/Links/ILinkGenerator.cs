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

internal class LinkGenerator<T>(
    IReadOnlyDictionary<Expression<Func<T, object>>, ILinkReference> links) : ILinkGenerator<T>
{
    private static Dictionary<string, ILink<T>> ResolveLinks(IReadOnlyDictionary<Expression<Func<T, object>>, ILinkReference> links)
    {
        var dictionary = new Dictionary<string, ILink<T>>();
        LinkExpressionVisitor l = new();
        foreach(var (key, value) in links)
        {
            l.Visit(key);
            if (l.MemberName is not null)
            {
                var relKey = value.Rel;
                if (string.IsNullOrWhiteSpace(relKey))
                {
                    relKey = l.MemberName;
                }

                var link = new Link<T>(value.Href, value.Method, value.Type, key)
                    .AddOrUpdateBag(relKey, l.MemberName);
                
                dictionary.Add(relKey, link);
            }
        }
        return dictionary;
    }

#pragma warning disable CA1859 //Link is not exposed outside of this class
    private static ILink ProduceLink(ILink<T> link, string key, T value)
    {
        var val = link.ValueExpression.Compile()(value);

        if(link.TryGetValue(key, out var aliasKey))
        {
            key = aliasKey;
        }

        var href = link.Href.Replace($"{{{key}}}", val.ToString() ?? string.Empty);
        return new Link(href, link.Method, link.Type);
    }
#pragma warning restore CA1859
    private readonly Dictionary<string, ILink<T>> links = ResolveLinks(links);

    public ILinkCollection GenerateLinks(T value)
    {
        return new LinkCollection(links.ToDictionary(k => k.Key, k => ProduceLink(k.Value, k.Key, value)));
    }
}
