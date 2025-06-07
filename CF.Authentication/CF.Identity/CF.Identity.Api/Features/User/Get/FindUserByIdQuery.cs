using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Get;

public record FindUserByIdQuery(Guid Id, bool Bypass = false) : IUnitRequest<UserDto>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => RoleRegistrar.List<UserRoles>(RoleCategory.Read);
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
