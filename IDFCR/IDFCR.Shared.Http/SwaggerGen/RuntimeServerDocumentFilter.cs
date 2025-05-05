using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        var httpContext = accessor.HttpContext;
        var addresses = httpContext?.Request?.Host;

        if (addresses is not null)
        {
            var scheme = accessor.HttpContext?.Request.Scheme;
            var url = $"{scheme}://{addresses}";
            swaggerDoc.Servers = [new OpenApiServer { Url = url }];
        }

        
    }
}

/// <summary>
/// Adds server addresses to Swagger generation, ensure you register the <see cref="IHttpContextAccessor"/> in your DI container.
/// <para>In order for this to work, ensure your configuration has <code>"OpenApi:Version":"3.0.x"</code></para>
/// </summary>
/// <param name="accessor"></param>
public class OpenApiVersionDocumentFilter(IHttpContextAccessor accessor) : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var httpContext = accessor.HttpContext;
        var config = httpContext?.RequestServices.GetRequiredService<IConfiguration>();

        string? openApiVersion;
        if (config is not null && (openApiVersion = config.GetValue<string>("OpenApi:Version")) != null)
        {
            swaggerDoc.Info.Version = openApiVersion;
        }
    }
}