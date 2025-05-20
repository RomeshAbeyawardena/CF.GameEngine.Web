using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IDFCR.Shared.Http.Mediatr;

public class RoleRequirementPrequestHandler<TRequest, TResponse>(ILogger<RoleRequirementPrequestHandler<TRequest, TResponse>> logger, 
    IHttpContextAccessor contextAccessor, IScopedStateReader scopedStateReader) : MediatR.Pipeline.IRequestPreProcessor<TRequest>
    where TRequest : IRequest<TResponse>, IRoleRequirement
    where TResponse : class
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if(await scopedStateReader.IsScopedBypassInvokedAsync(cancellationToken) || request.Bypass)
        {
            logger.LogWarning("Bypassing role requirement for request {RequestType}, ensure this was not used by a front-end facing endpoint that required authorisation", 
                request.GetType().Name);
            return;
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
            return;
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
    }
}

