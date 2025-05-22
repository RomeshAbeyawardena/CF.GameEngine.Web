using IDFCR.Shared.Http.Extensions;
using IDFCR.Shared.Http.Mediatr.Scopes;
using Microsoft.AspNetCore.Http;

namespace IDFCR.Shared.Http.Mediatr;

public enum RoleRequirementHandlerInterceptorType
{
    Bypass = 1,
    Extension = 2
}

public interface IRoleRequirementHandlerInterceptor<TRequest>
{
    RoleRequirementHandlerInterceptorType Type { get; }
    Task<bool> CanInterceptAsync(IHttpContextWrapper context, TRequest request, CancellationToken cancellationToken);
    Task<bool> InterceptAsync(IHttpContextWrapper context, TRequest request, CancellationToken cancellationToken);
}


        
public class ScopeStateRoleRequirementInterceptor<TRequest>(IScopedStateReader scopedStateReader) : IRoleRequirementHandlerInterceptor<TRequest>
{
    public RoleRequirementHandlerInterceptorType Type => RoleRequirementHandlerInterceptorType.Bypass;

    public async Task<bool> CanInterceptAsync(IHttpContextWrapper context, TRequest request, CancellationToken cancellationToken)
    {
        return await scopedStateReader.Contains(ScopeConstants.ScopedBypassKey);
    }

    public async Task<bool> InterceptAsync(IHttpContextWrapper context, TRequest request, CancellationToken cancellationToken)
    {
        return await scopedStateReader.IsScopedBypassInvokedAsync(cancellationToken);
    }
}