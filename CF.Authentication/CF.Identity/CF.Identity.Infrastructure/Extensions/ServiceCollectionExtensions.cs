using IDFCR.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CF.Identity.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection EnumerateRoles(this IServiceCollection services)
    {
        return services.AddDynamicRoles<JwtSettings>();
    }
}
