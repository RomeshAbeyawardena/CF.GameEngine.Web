using System.Linq.Expressions;

namespace IDFCR.Shared.Http.Links;

public interface ILinkPlaceholderBag : IReadOnlyDictionary<string, string>
{
    
}

public interface ILink<T> : ILink
{
    string? RouteName { get; }
    //ILink<T> AddOrUpdateBag(string key, string value);
    IEnumerable<Expression<Func<T, object>>> ValueExpressions { get; }
}

public interface ILinkReference<T> : ILink<T>
{
    string? Rel { get; }
}


public interface ILink
{
    string? Href { get; }
    string Method { get; }
    string Type { get; }
}
