using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace IDFCR.Http.Authentication.Extensions;

public static class ServiceCollectionExtensions
{
    public static AuthenticationBuilder AddScopeBasedAuthorization(this IServiceCollection services, string defaultScheme, params string[] defaultScopes)
    {
        return services.AddSingleton<IDefaultScopeClaimPolicy>(new DefaultScopeClaimPolicy(defaultScheme, defaultScopes))
            .AddSingleton<IAuthorizationPolicyProvider, ScopeClaimPolicyProvider>()
            .AddSingleton<IAuthorizationHandler, ScopeClaimPolicyHandler>()
            .AddAuthentication(defaultScheme);
    }
}
