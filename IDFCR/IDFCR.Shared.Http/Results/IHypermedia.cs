using IDFCR.Shared.Http.Links;
using System.Text.Json.Serialization;

namespace IDFCR.Shared.Http.Results;

public interface IHypermedia : IReadOnlyDictionary<string, object?>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    IReadOnlyDictionary<string, ILink?>? Links { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull | JsonIgnoreCondition.WhenWritingDefault)]
    IReadOnlyDictionary<string, object?>? Meta { get; }
}

public interface IHypermedia<T> : IHypermedia
{
    T? Value { get; }
}
