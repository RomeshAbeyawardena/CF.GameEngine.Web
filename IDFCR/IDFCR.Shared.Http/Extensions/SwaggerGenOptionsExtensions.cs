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
    public static SwaggerGenOptions AddRuntimeServerDocumentFilter(this SwaggerGenOptions options)
    {
        options.DocumentFilter<RuntimeServerDocumentFilter>();
        return options;
    }
}

