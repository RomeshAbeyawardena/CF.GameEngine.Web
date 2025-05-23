using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IDFCR.Http.Authentication.Abstractions;

public interface IAuthenticatedUserContext
{
    string? AccessToken { get; }
    ClientInfo? Client { get; }
    ClaimsPrincipal? User { get; }
}

public class AuthenticatedUserContext(IHttpContextAccessor contextAccessor) : IAuthenticatedUserContext
{
    public ClaimsPrincipal? User => contextAccessor?.HttpContext?.User;
    public ClientInfo? Client => User?.GetClient();
    public string? AccessToken => User?.GetAccessToken();
}
