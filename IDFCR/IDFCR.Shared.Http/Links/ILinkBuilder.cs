using Microsoft.AspNetCore.Routing;
using System.Linq.Expressions;

namespace IDFCR.Shared.Http.Links;

public interface ILinkBuilder<T>
{
    ILinkBuilder<T> AddSelf(string hrefTemplate, string method = "GET", string type = "application/json", params Expression<Func<T, object>>[] expressions);
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
    ILinkBuilder<T> AddLink(string hrefTemplate, string method = "GET", string type = "application/json", string? rel = null, params Expression<Func<T, object>>[] expressions);
    ILinkGenerator<T> Build(LinkGenerator linkGenerator);
}

public abstract class DeferredLinkBuilder<T> : LinkBuilder<T>
{
    public ILinkBuilder<T> AddLink(Expression<Func<T, object>> source, string routeName, string method = "GET", string type = "application/json", string? rel = null,
        params Expression<Func<T, object>>[] expressions)
    {
        LinkDictionary.Add(source, new LinkReference<T>(null, method, type, expressions, rel, routeName));
        return this;
    }
}

public abstract class LinkBuilder<T> : ILinkBuilder<T>
{
    private readonly Dictionary<Expression<Func<T, object>>, ILinkReference<T>> _linkDictionary = [];
    protected IDictionary<Expression<Func<T, object>>, ILinkReference<T>> LinkDictionary => _linkDictionary;

    public ILinkBuilder<T> AddLink(string hrefTemplate, string method = "GET", string type = "application/json", string? rel = null, params Expression<Func<T, object>>[] expressions)
    {
        _linkDictionary.Add(expressions.FirstOrDefault() ?? throw new NullReferenceException("Must have an expression"), 
            new LinkReference<T>(hrefTemplate, method, type, expressions, rel));
        return this;
    }

    public ILinkBuilder<T> AddSelf(string hrefTemplate, string method = "GET", string type = "application/json", params Expression<Func<T, object>>[] expressions)
    {
        return AddLink(hrefTemplate, method, type, "_self", expressions);
    }

    public ILinkGenerator<T> Build(LinkGenerator linkGenerator)
    {
        return new LinkGenerator<T>(linkGenerator, _linkDictionary);
    }
}