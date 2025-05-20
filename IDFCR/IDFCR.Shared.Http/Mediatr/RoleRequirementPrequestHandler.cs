using IDFCR.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IDFCR.Shared.Http.Mediatr;

public class RoleRequirementPrequestHandler<TRequest, TResponse>(ILogger<RoleRequirementPrequestHandler<TRequest, TResponse>> logger, IHttpContextAccessor contextAccessor) : MediatR.Pipeline.IRequestPreProcessor<TRequest>
    where TRequest : IRequest<TResponse>, IRoleRequirement
    where TResponse : class
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if(request.Bypass)
        {
            logger.LogWarning("Bypassing role requirement for request {RequestType}, ensure this was not used by a front-end facing endpoint that required authorisation", 
                request.GetType().Name);
            return Task.CompletedTask;
        }

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

