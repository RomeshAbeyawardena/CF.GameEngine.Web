using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Assign;

public record AssignUserScopesCommand(Guid ClientId, Guid UserId, IEnumerable<string> Scopes, bool Bypass = false) 
    : RoleRequirementBase(() => RoleRegistrar.List<UserRoles>(RoleCategory.Write)), IUnitRequest
{

}
