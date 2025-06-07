using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;

using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;

namespace CF.Identity.Api.Features.AccessRoles.Find;

public record FindAccessRoleQuery(Guid AccessRoleId, bool Bypass = false) 
    : RoleRequirementBase(() => RoleRegistrar.List<Roles>(RoleCategory.Read)), IUnitRequest<AccessRoleDetail>
{
}
