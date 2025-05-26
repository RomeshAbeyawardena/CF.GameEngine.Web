using IDFCR.Shared.Http.Links;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace IDFCR.Shared.Http.Results;

public record HypermediaApiListResult<T>(IEnumerable<T> RawData, int StatusCode)
    : ApiResult<IHypermediaCollection<T>>(new HypermediaCollection<T>(RawData), StatusCode)
{
    protected override void OnExecuteAsync(HttpContext httpContext)
    {
        base.OnExecuteAsync(httpContext);
        var services = httpContext.RequestServices;
        var linkBuilders = services.GetServices<ILinkBuilder<T>>();
        var firstBuilder = linkBuilders.FirstOrDefault();

        if (firstBuilder is not null && Data is not null)
        {
            if (linkBuilders.Count() > 1)
            {
                firstBuilder.Merge(linkBuilders.Skip(1));
            }

            foreach (var entry in Data)
            {
                var wrapper = entry as Hypermedia<T> ?? throw new InvalidCastException("Not a wrapper of Hypermedia");

                if (entry.Value is not null)
                {
                    var links = firstBuilder.Build(
                        services.GetRequiredService<LinkGenerator>()).GenerateLinks(entry.Value);

                    foreach (var (key, value) in links)
                    {
                        if (!wrapper._links.TryAdd(key, value))
                        {
                            wrapper._links[key] = value;
                        }
                    }
                }
            }
        }
        //if there are no builders processing continues without links
    }
}

public record HypermediaApiResult<T>(T? RawData, int StatusCode) : ApiResult<IHypermedia<T>>(new Hypermedia<T>(RawData), StatusCode), IApiResult<IHypermedia<T>>
{
    protected override void OnExecuteAsync(HttpContext httpContext)
    {
        base.OnExecuteAsync(httpContext);
        var services = httpContext.RequestServices;
        var linkBuilders = services.GetServices<ILinkBuilder<T>>();
        var firstBuilder = linkBuilders.FirstOrDefault();

        if (firstBuilder is not null)
        {
            if (linkBuilders.Count() > 1)
            {
                firstBuilder.Merge(linkBuilders.Skip(1));
            }

            if (Data.Value is not null)
            {
                var links = firstBuilder.Build(
                    services.GetRequiredService<LinkGenerator>()).GenerateLinks(Data.Value);

                var wrapper = Data as Hypermedia<T> ?? throw new InvalidCastException("Not a wrapper of Hypermedia");

                foreach (var (key, value) in links)
                {
                    if (!wrapper._links.TryAdd(key, value))
                    {
                        wrapper._links[key] = value;
                    }
                }
            }
        }
        //if there are no builders processing continues without links
    }
}