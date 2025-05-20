using IDFCR.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IDFCR.Shared.Http.Mediatr;

public class RoleRequirementPrequestHandler<TRequest, TResponse>(IHttpContextAccessor contextAccessor) : MediatR.Pipeline.IRequestPreProcessor<TRequest>
    where TRequest : IRequest<TResponse>, IRoleRequirement
    where TResponse : class
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var context = contextAccessor.HttpContext ?? throw new InvalidOperationException("This is not running in a valid HttpContext");
        var user = context.User;
        var identity = user.Identity;
        if (identity is null || !identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        var roles = request.Roles;
        if(roles is null || !roles.Any())
        {
            return Task.CompletedTask;
        }

        if(request.RoleRequirementType == RoleRequirementType.All
            && !roles.All(user.IsInRole))
        {
            throw new UnauthorizedAccessException("User is not authorised");
        }

        if (request.RoleRequirementType == RoleRequirementType.Some
            && !roles.Any(user.IsInRole))
        {
            throw new UnauthorizedAccessException("User is not authorised");
        }

        return Task.CompletedTask;
    }
}

