using System.Linq.Expressions;

namespace IDFCR.Shared.Http.Links;

internal record LinkReference(string Href, string Method, string Type, string Rel = "_self") : ILinkReference;

internal record Link<T>(string Href, string Method, string Type, Expression<Func<T, object>> ValueExpression) 
    : Link(Href, Method, Type), ILink<T>;

internal record Link(string Href, string Method, string Type) : ILink;