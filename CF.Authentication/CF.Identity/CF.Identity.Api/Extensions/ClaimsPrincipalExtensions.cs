using System.Security.Claims;

namespace CF.Identity.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal user) =>
    user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public static string? GetUserDisplayName(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.GivenName)?.Value;

    public static string? GetUserName(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.Name)?.Value;

    public static string? GetUserEmail(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.Email)?.Value;

}
