using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace IDFCR.Shared.Http;

public interface IHttpContextWrapper
{
    [MemberNotNullWhen(true, nameof(Context))]
    bool IsValid { get; }
    
    HttpContext? Context { get; }
}

public record HttpContextWrapper(IHttpContextAccessor ContextAccessor) : IHttpContextWrapper
{
    [MemberNotNullWhen(true, nameof(Context))]
    public bool IsValid => ContextAccessor.HttpContext is not null;
    public HttpContext? Context => ContextAccessor.HttpContext;
}