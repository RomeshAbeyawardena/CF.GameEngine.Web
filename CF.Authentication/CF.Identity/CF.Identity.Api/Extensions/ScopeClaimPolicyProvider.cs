using CF.Identity.Api.Features;
using Microsoft.AspNetCore.Authorization;
namespace CF.Identity.Api.Extensions;

public record ScopeClaimPolicy(string? Scope = null, IEnumerable<string>? Scopes = null) : IAuthorizationRequirement;

public class ScopeClaimPolicyHandler : AuthorizationHandler<ScopeClaimPolicy>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopeClaimPolicy requirement)
    {
        List<string> scopes = requirement.Scopes?.ToList() ?? [];

        if (!string.IsNullOrWhiteSpace(requirement.Scope))
        {
            scopes.Add(requirement.Scope);
        }

        foreach (var scope in scopes)
        {
            // Check if the user has the required scope claim
            if (context.User.IsInRole(scope))
            {
                context.Succeed(requirement);
                //as long as it has one, it should pass.
                return Task.CompletedTask;
            }
        }

        context.Fail(new AuthorizationFailureReason(this, "User does not have access to this role"));
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
        IEnumerable<string>? scopes = null;
        if (policyName.Contains(','))
        {
            //allow a maximum of five scopes, because anything beyond would be exessive, might as well call it god user and be done with it.
            scopes = policyName.Split(',', 6, StringSplitOptions.RemoveEmptyEntries);
        }

        return new ([new ScopeClaimPolicy(policyName, scopes)], ["ClientBearer"]);
    }
}
