using System.Linq.Expressions;

namespace IDFCR.Shared.Http.Links;

public interface ILinkBuilder<T>
{
    ILinkBuilder<T> AddSelf(Expression<Func<T, object>> source, string hrefTemplate, string method = "GET", string type = "application/json");
    /// <summary>
    /// Resolves to _links: { "{rel}": { "href": "{href_with_replacements_to_placeholders}", "method":"{method}", "type":"{type}" } }
    /// </summary>
    /// <typeparam name="TProp"></typeparam>
    /// <param name="source"></param>
    /// <param name="rel"></param>
    /// <param name="href"></param>
    /// <param name="method"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    ILinkBuilder<T> AddLink(Expression<Func<T,object>> source, string hrefTemplate, string method = "GET", string type = "application/json", string? rel = null);
    ILinkGenerator<T> Build();
}

public abstract class LinkBuilder<T> : ILinkBuilder<T>
{
    private readonly Dictionary<Expression<Func<T, object>>, ILinkReference> _linkDictionary = [];

    public ILinkBuilder<T> AddLink(Expression<Func<T, object>> source, string hrefTemplate, string method = "GET", string type = "application/json", string? rel = null)
    {
        _linkDictionary.Add(source, new LinkReference(hrefTemplate, method, type, rel));
        return this;
    }

    public ILinkBuilder<T> AddSelf(Expression<Func<T, object>> source, string hrefTemplate, string method = "GET", string type = "application/json")
    {
        _linkDictionary.Add(source, new LinkReference(hrefTemplate, method, type, "_self"));
        return this;
    }

    public ILinkGenerator<T> Build()
    {
        return new LinkGenerator<T>(_linkDictionary);
    }
}