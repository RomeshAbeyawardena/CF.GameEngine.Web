using Microsoft.AspNetCore.Authorization;
namespace IDFCR.Http.Authentication;

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
