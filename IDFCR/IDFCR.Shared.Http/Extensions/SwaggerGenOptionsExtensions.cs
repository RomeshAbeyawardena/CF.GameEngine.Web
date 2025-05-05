using IDFCR.Shared.Http.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IDFCR.Shared.Http.Extensions;

public static class SwaggerGenOptionsExtensions
{
    /// <summary>
    /// Adds server addresses to Swagger generation, ensure you register the <see cref="IHttpContextAccessor"/> in your DI container.
    /// <code>Services.AddHttpContextAccessor();</code>
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static SwaggerGenOptions UseRuntimeServer(this SwaggerGenOptions options)
    {
        options.DocumentFilter<RuntimeServerDocumentFilter>();
        return options;
    }

    /// <summary>
    /// Adds server addresses to Swagger generation, ensure you register the <see cref="IHttpContextAccessor"/> in your DI container.
    /// <para>In order for this to work, ensure your configuration has <code>"OpenApi:Version":"3.0.x"</code></para>
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public static SwaggerGenOptions UseOpenApiVersionFromConfig(this SwaggerGenOptions options)
    {
        options.DocumentFilter<OpenApiVersionDocumentFilter>();
        return options;
    }
}

