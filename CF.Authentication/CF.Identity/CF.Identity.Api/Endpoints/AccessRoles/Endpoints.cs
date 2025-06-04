using CF.Identity.Api.Endpoints.AccessRoles.Get;
using CF.Identity.Api.Endpoints.AccessRoles.Post;

namespace CF.Identity.Api.Endpoints.AccessRoles;

public static class Endpoints
{
    public const string BaseUrl = "api/role";
    public const string ListAccessRoles = nameof(ListAccessRoles);
    public const string GetAccessRole = nameof(GetAccessRole);
    public static IEndpointRouteBuilder AddAccessRoleEndpoints(this IEndpointRouteBuilder builder)
    {
        return builder.AddGetRolesEndpoint()
            .AddPostRoleEndpoint();
    }
}
