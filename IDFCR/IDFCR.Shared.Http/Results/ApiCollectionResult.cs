using IDFCR.Shared.Http.Extensions;
using IDFCR.Shared.Http.Links;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace IDFCR.Shared.Http.Results;

public record ApiCollectionResult<T>
    : ApiResult<IEnumerable<IEntryWrapper<T>>>, IApiResult<IEnumerable<IEntryWrapper<T>>>
{
    private readonly bool _buildNestedLinks;
    public ApiCollectionResult(IEnumerable<T> Data, int StatusCode, bool buildNestedLinks = true)
        : base(Data.Select(x => x.AsEntryWrapper()), StatusCode, false)
    {
        _buildNestedLinks = buildNestedLinks;
    }

    protected override void OnExecuteAsync(HttpContext httpContext)
    {
        base.OnExecuteAsync(httpContext);
        if (this._buildNestedLinks)
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
                            services.GetRequiredService<LinkGenerator>()).GenerateLinks(entry.Entry);

                        foreach (var (key, value) in links)
                        {
                            entry.AddLink(key, value);
                        }
                    }
                }
            }
        }
    }
}
