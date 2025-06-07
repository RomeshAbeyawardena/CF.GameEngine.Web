using CF.Identity.Infrastructure.Features.AccessRoles;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.AccessRoles.Post;

public record PostAccessRoleCommand(EditableAccessRoleDto AccessRole, bool Bypass = false) 
    : RoleRequirementBase(() => RoleRegistrar.List<Roles>(RoleCategory.Write)), IUnitRequest<Guid>
{
    
    
}
