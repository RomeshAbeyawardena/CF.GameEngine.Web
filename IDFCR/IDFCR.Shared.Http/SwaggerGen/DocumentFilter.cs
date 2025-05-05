using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IDFCR.Shared.Http.SwaggerGen;

/// <summary>
/// Adds server addresses to Swagger generation, ensure you register the <see cref="IHttpContextAccessor"/> in your DI container.
/// <code>Services.AddHttpContextAccessor();</code>
/// </summary>
/// <param name="accessor"></param>
public class RuntimeServerDocumentFilter(IHttpContextAccessor accessor) : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var addresses = accessor.HttpContext?.Request?.Host;

        if (addresses is not null)
        {
            var scheme = accessor.HttpContext?.Request.Scheme;
            var url = $"{scheme}://{addresses}";
            swaggerDoc.Servers = [new OpenApiServer { Url = url }];
        }
    }
}
