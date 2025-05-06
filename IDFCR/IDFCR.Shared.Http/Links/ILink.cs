using System.Linq.Expressions;

namespace IDFCR.Shared.Http.Links;

public interface ILinkReference : ILink
{
    string? Rel { get; }
}

public interface ILinkPlaceholderBag : IReadOnlyDictionary<string, string>
{
    
}

public interface ILink<T> : ILink, ILinkPlaceholderBag
{
    ILink<T> AddOrUpdateBag(string key, string value);
    Expression<Func<T, object>> ValueExpression { get; }
}

public interface ILink
{
    string Href { get; }
    string Method { get; }
    string Type { get; }
}
