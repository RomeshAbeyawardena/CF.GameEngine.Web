using IDFCR.Shared.Http.Links;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace IDFCR.Shared.Http.Results;

public record ApiCollectionResult<T>(IEnumerable<T> Data, int StatusCode, bool BuildNestedLinks = true) : ApiResult<IEnumerable<T>>(Data, StatusCode, false), IApiResult<IEnumerable<T>>
{
    protected override void OnExecuteAsync(HttpContext httpContext)
    {
        base.OnExecuteAsync(httpContext);
        if (this.BuildNestedLinks)
        {
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
                    
                    if (entry is not null)
                    {
                        var links = firstBuilder.Build(
                            services.GetRequiredService<LinkGenerator>()).GenerateLinks(entry);

                        foreach (var (key, value) in links)
                        {
                            if (!MutableLinks.TryAdd(key, value))
                            {
                                MutableLinks[key] = value;
                            }
                        }
                    }
                }
            }
        }
    }
}
