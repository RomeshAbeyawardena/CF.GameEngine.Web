using CF.Identity.Infrastructure.Features.AccessRoles;
using IDFCR.Shared.Abstractions;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessRoles.Upsert;

public record UpsertAccessRoleCommand(EditableAccessRoleDto AccessRole, bool Bypass = false) 
    : RoleRequirementBase(() => RoleRegistrar.List<Roles>(RoleCategory.Write)), IUnitRequest<Guid>;