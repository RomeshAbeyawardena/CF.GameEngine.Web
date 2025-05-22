using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace IDFCR.Http.Authentication.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScopeBasedAuthorization(this IServiceCollection services, string defaultScheme, params string[] defaultScopes)
    {
        services.AddSingleton<IDefaultScopeClaimPolicy>(new DefaultScopeClaimPolicy(defaultScheme, defaultScopes));
        services.AddSingleton<IAuthorizationPolicyProvider, ScopeClaimPolicyProvider>();
        
        return services;
    }
}
