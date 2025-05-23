using Microsoft.AspNetCore.Authorization;
namespace IDFCR.Http.Authentication;

public class ScopeClaimPolicyProvider(IDefaultScopeClaimPolicy defaultScopeClaimPolicy) : IAuthorizationPolicyProvider
{
    private readonly AuthorizationPolicy Default = new([new ScopeClaimPolicy(Scopes: defaultScopeClaimPolicy.DefaultScopes)], [defaultScopeClaimPolicy.DefaultScheme]);
    public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        await Task.CompletedTask;
        return Default;
    }

    public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        await Task.CompletedTask;
        return null;
    }

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        await Task.CompletedTask;
        IEnumerable<string>? scopes = null;
        if (policyName.Contains(','))
        {
            //allow a maximum of five scopes, because anything beyond would be exessive, might as well call it god user and be done with it.
            scopes = policyName.Split(',', 6, StringSplitOptions.RemoveEmptyEntries);
        }
        
        return new ([new ScopeClaimPolicy(policyName, scopes)], [defaultScopeClaimPolicy.DefaultScheme]);
    }
}
