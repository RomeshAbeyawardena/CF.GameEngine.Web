using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Post;

public record PostUserCommand(EditableUserDto User) : IUnitRequest<Guid>, IRoleRequirement
{
    bool IRoleRequirement.Bypass => false;
    IEnumerable<string> IRoleRequirement.Roles => RoleRegistrar.List<UserRoles>(RoleCategory.Write);
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
