using CF.Identity.Api.Features;
using Microsoft.AspNetCore.Authorization;
namespace CF.Identity.Api.Extensions;

public record ScopeClaimPolicy(string Scope) : IAuthorizationRequirement;

public class ScopeClaimPolicyHandler : AuthorizationHandler<ScopeClaimPolicy>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeClaimPolicy requirement)
    {
        // Check if the user has the required scope claim
        if (context.User.IsInRole(requirement.Scope))
        {
            context.Succeed(requirement);
        }

        context.Fail();
        return Task.CompletedTask;
    }
}

public class ScopeClaimPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly AuthorizationPolicy Default = new([new ScopeClaimPolicy(Roles.GlobalRead)], ["ClientBearer"]);
    public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        await Task.CompletedTask;
        return Default;
    }

    public async Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        await Task.CompletedTask;
        return Default;
    }

    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        await Task.CompletedTask;
        return new ([new ScopeClaimPolicy(policyName)], ["ClientBearer"]);
    }
}
