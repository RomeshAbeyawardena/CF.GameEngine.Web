using IDFCR.Shared.Http.Abstractions;
using IDFCR.Shared.Http.Links;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Text.Json.Serialization;

namespace IDFCR.Shared.Http.Results;

public interface IApiResult : IResult
{
    int StatusCode { get; }
    DateTimeOffset RequestedTimestampUtc { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull
        | JsonIgnoreCondition.WhenWritingDefault)]
    IError? Error { get; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull
        | JsonIgnoreCondition.WhenWritingDefault)]
    IReadOnlyDictionary<string, object?>? Meta { get; }

    IApiResult AddHeader(string name, StringValues values);
    IApiResult AppendMeta(IDictionary<string, object?> values);
}

public interface IApiResult<T> : IApiResult
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull
        | JsonIgnoreCondition.WhenWritingDefault)]
    IReadOnlyDictionary<string, ILink>? Links { get; }
    T Data { get; }
}
