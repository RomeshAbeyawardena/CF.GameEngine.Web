using IDFCR.Shared.Exceptions;
using IDFCR.Shared.Http.Abstractions;
using IDFCR.Shared.Http.Extensions;
using IDFCR.Shared.Http.Links;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace IDFCR.Shared.Http.Results;

public record HypermediaApiResult<T>(T RawData, int StatusCode) : ApiResult<IHypermedia<T>>(new Hypermedia<T>(RawData), StatusCode), IApiResult<IHypermedia<T>>
{
    //protected override void OnExecuteAsync(HttpContext httpContext)
    //{
    //    base.OnExecuteAsync(httpContext);
    //    //var services = httpContext.RequestServices;
    //    //var linkBuilders = services.GetServices<ILinkBuilder<T>>();
    //    //var firstBuilder = linkBuilders.FirstOrDefault();

    //    //if(firstBuilder is not null)
    //    //{
    //    //    if(linkBuilders.Count() > 1)
    //    //    {
    //    //        firstBuilder.Merge(linkBuilders.Skip(1));
    //    //    }

    //    //    var links = firstBuilder.Build(
    //    //        services.GetRequiredService<LinkGenerator>()).GenerateLinks(Data);

    //    //    foreach (var (key, value) in links)
    //    //    {
    //    //        if (!Links.TryAdd(key, value))
    //    //        {
    //    //            Links[key] = value;
    //    //        }
    //    //    }
    //    //}
    //   //if there are no builders processing continues without links
    //}
}


public record ApiResult<T>(T Data, int StatusCode) : ApiResult(StatusCode), IApiResult<T>
{
    //protected override void OnExecuteAsync(HttpContext httpContext)
    //{
    //    base.OnExecuteAsync(httpContext);
    //    //var services = httpContext.RequestServices;
    //    //var linkBuilders = services.GetServices<ILinkBuilder<T>>();
    //    //var firstBuilder = linkBuilders.FirstOrDefault();

    //    //if(firstBuilder is not null)
    //    //{
    //    //    if(linkBuilders.Count() > 1)
    //    //    {
    //    //        firstBuilder.Merge(linkBuilders.Skip(1));
    //    //    }
            
    //    //    var links = firstBuilder.Build(
    //    //        services.GetRequiredService<LinkGenerator>()).GenerateLinks(Data);

    //    //    foreach (var (key, value) in links)
    //    //    {
    //    //        if (!Links.TryAdd(key, value))
    //    //        {
    //    //            Links[key] = value;
    //    //        }
    //    //    }
    //    //}
    //   //if there are no builders processing continues without links
    //}

    public override async Task ExecuteAsync(HttpContext httpContext)
    {
        OnExecuteAsync(httpContext);
        await httpContext.Response.WriteAsJsonAsync<IApiResult<T>>(this);
    }
}

public record ApiResult(int StatusCode, Exception? Exception = null)
    : IApiResult
{
    private readonly Dictionary<string, StringValues> _rewrittenHeaders = [];
    private readonly Dictionary<string, object?> _meta = [];
    private readonly Dictionary<string, object?> _links = [];

    protected IDictionary<string, object?> Links => _links;

    protected virtual void OnExecuteAsync(HttpContext httpContext)
    {
        var timeProvider = httpContext.RequestServices.GetRequiredService<TimeProvider>();
        RequestedTimestampUtc = timeProvider.GetUtcNow();
        httpContext.Response.StatusCode = StatusCode;

        foreach (var (header, value) in _rewrittenHeaders)
        {
            httpContext.Response.Headers[header] = value;
        }
    }

    public DateTimeOffset RequestedTimestampUtc { get; private set; }

    public IError? Error => Exception == null
        ? null
        : Exception is IExposableException exposableException 
            ? new Error(exposableException)
            : new Error(Exception.Message, Exception.StackTrace);

    IReadOnlyDictionary<string, object?>? IApiResult.Links => _links.Count > 0 ? _links : null;
    public IReadOnlyDictionary<string, object?>? Meta => _meta.Count > 0 ? _meta : null;

    public IApiResult AddHeader(string name, StringValues values)
    {
        _rewrittenHeaders[name] = values;
        return this;
    }

    public IApiResult AppendMeta(IDictionary<string, object?> values)
    {
        foreach (var (key, value) in values)
        {
            if (!_meta.TryAdd(key, value))
            {
                _meta[key] = value;
            }
        }
        return this;
    }

    public virtual async Task ExecuteAsync(HttpContext httpContext)
    {
        OnExecuteAsync(httpContext);
        await httpContext.Response.WriteAsJsonAsync<IApiResult>(this);
    }
}
