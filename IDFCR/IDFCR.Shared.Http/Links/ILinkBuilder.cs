using Microsoft.AspNetCore.Routing;
using System.Linq.Expressions;

namespace IDFCR.Shared.Http.Links;

/// <summary>
/// Represents a marker interface
/// </summary>
public interface ILinkBuilder;
public interface ILinkBuilder<T> : ILinkBuilder
{
    ILinkBuilder<T> AddSelf(string hrefTemplate, string method = "GET", string type = "application/json", params Expression<Func<T, object>>[] expressions);
    ILinkBuilder<T> AddSelf(string hrefTemplate, params Expression<Func<T, object>>[] expressions);
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
    ILinkBuilder<T> AddLink(string hrefTemplate, params Expression<Func<T, object>>[] expressions);
    ILinkGenerator<T> Build(LinkGenerator linkGenerator);
}

public abstract class DeferredLinkBuilder<T> : LinkBuilder<T>
{
    public ILinkBuilder<T> AddDeferredLink(string routeName, string method = "GET", string type = "application/json", string? rel = null,
        params Expression<Func<T, object>>[] expressions)
    {
        LinkDictionary.Add(expressions.FirstOrDefault() ?? throw new NullReferenceException("Must have an expression"), 
            new LinkReference<T>(null, method, type, expressions, rel, routeName));
        return this;
    }

    public ILinkBuilder<T> AddDeferredSelfLink(string routeName, string method = "GET", string type = "application/json", params Expression<Func<T, object>>[] expressions)
    {
        return AddDeferredLink(routeName, method, type, "_self", expressions);
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

    public ILinkBuilder<T> AddLink(string hrefTemplate, params Expression<Func<T, object>>[] expressions)
    {
        return AddLink(hrefTemplate, expressions: expressions);
    }

    public ILinkBuilder<T> AddSelf(string hrefTemplate, string method = "GET", string type = "application/json", params Expression<Func<T, object>>[] expressions)
    {
        return AddLink(hrefTemplate, method, type, "_self", expressions);
    }

    public ILinkBuilder<T> AddSelf(string hrefTemplate, params Expression<Func<T, object>>[] expressions)
    {
        return AddSelf(hrefTemplate, expressions: expressions);
    }

    public ILinkGenerator<T> Build(LinkGenerator linkGenerator)
    {
        return new LinkGenerator<T>(linkGenerator, _linkDictionary);
    }
}