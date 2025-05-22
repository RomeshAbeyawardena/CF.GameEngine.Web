using IDFCR.Shared.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IDFCR.Shared.Http.Mediatr;

public class RoleRequirementPrequestHandler<TRequest, TResponse>(ILogger<RoleRequirementPrequestHandler<TRequest, TResponse>> logger,
    IHttpContextWrapper contextAccessor, IEnumerable<IRoleRequirementHandlerInterceptor<TRequest>> roleRequirementHandlerInterceptors) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>, IRoleRequirement
    where TResponse : class
{
    private async Task<bool> RunAllAsync(IEnumerable<IRoleRequirementHandlerInterceptor<TRequest>> interceptors, TRequest request, CancellationToken cancellationToken)
    {
        List<bool> interceptorResults = [];
        
        foreach (var interceptor in interceptors)
        {
            if (await interceptor.CanInterceptAsync(contextAccessor, request, cancellationToken))
            {
                var interceptorType = interceptor.GetType();
                logger.LogTrace("[{type}]: Interceptor of type {interceptorType} executed", interceptor.Type, interceptorType);
                bool result;
                interceptorResults.Add(result = await interceptor.InterceptAsync(contextAccessor, request, cancellationToken));

                logger.LogTrace("Interceptor of type {type}: Validation {result}", interceptor.GetType(), result ? "passed" : "failed");
            }
        }

        return interceptorResults.TrueForAll(x => x);
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!contextAccessor.IsValid)
        {
            throw new InvalidOperationException("This is not running in a valid HttpContext");
        }

        var context = contextAccessor.Context;

        var byPassInterceptors = roleRequirementHandlerInterceptors
            .Where(x => x.Type == RoleRequirementHandlerInterceptorType.Bypass);

        if (await RunAllAsync(byPassInterceptors, request, cancellationToken) || request.Bypass)
        {
            logger.LogWarning("Bypassing role requirement for request {RequestType}, ensure this was not used by a front-end facing endpoint that required authorisation",
                request.GetType().Name);
            return await next(cancellationToken);
        }

        var user = context.User;
        var identity = user.Identity;
        if (identity is null || !identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        var roles = request.Roles;
        if (roles is null || !roles.Any())
        {
            return await next(cancellationToken);
        }

        if (request.RoleRequirementType == RoleRequirementType.All
            && !roles.All(user.IsInRole))
        {
            throw new UnauthorizedAccessException("User is not authorised");
        }

        if (request.RoleRequirementType == RoleRequirementType.Some
            && !roles.Any(user.IsInRole))
        {
            throw new UnauthorizedAccessException("User is not authorised");
        }

        var interceptInterceptors = roleRequirementHandlerInterceptors
            .Where(x => x.Type == RoleRequirementHandlerInterceptorType.Extension);

        if (await RunAllAsync(interceptInterceptors, request, cancellationToken))
        {
            return await next(cancellationToken);
        }

        throw new UnauthorizedAccessException("User is not authorised");
    }
}