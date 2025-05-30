using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Get;

public record FindUserByIdQuery(Guid Id, bool Bypass = false) : IUnitRequest<UserDto>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalRead, UserRoles.UserRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
