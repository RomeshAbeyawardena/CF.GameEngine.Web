using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Get;

public record GetUserByIdQuery(Guid Id, bool Bypass = false) : IUnitRequest<UserDto>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [Roles.GlobalRead, Roles.UserRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
