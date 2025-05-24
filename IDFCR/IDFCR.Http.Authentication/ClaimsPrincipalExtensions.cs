using System.Security.Claims;

namespace IDFCR.Http.Authentication;

public record ClientInfo(string? ClientId, string? ClientName, string? UserIdentifier, bool IsSystem)
{
    public Guid? Id => !string.IsNullOrWhiteSpace(ClientId) 
        && Guid.TryParse(ClientId, out var id) ? id : null;
    public Guid? UserId => !string.IsNullOrWhiteSpace(UserIdentifier)
        && Guid.TryParse(UserIdentifier, out var id) ? id : null;
}

public static class ClaimsPrincipalExtensions
{
    public static ClientInfo GetClient(this ClaimsPrincipal user)
    {
        var (clientId, clientName) = user.GetClientInfo();
        var systemClaim = user.FindFirst(ClaimTypes.System);
        var isSystem = bool.TryParse(systemClaim?.Value, out var isSys) && isSys;

        return new ClientInfo(clientId, clientName, user.GetUserId(), isSystem);
    }

    public static (string?, string?) GetClientInfo(this ClaimsPrincipal user) =>
        (user.FindFirst(ClaimTypes.GroupSid)?.Value, user.FindFirst(ClaimTypes.Sid)?.Value);

    public static string GetAccessToken(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.Authentication)?.Value ?? string.Empty;

    public static IEnumerable<string> GetScopes(this ClaimsPrincipal user) =>
        user.FindAll(ClaimTypes.Role).Select(x => x.Value);

    public static string? GetUserId(this ClaimsPrincipal user) =>
    user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public static string? GetUserDisplayName(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.GivenName)?.Value;

    public static string? GetUserName(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.Name)?.Value;

    public static string? GetUserEmail(this ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.Email)?.Value;

}
