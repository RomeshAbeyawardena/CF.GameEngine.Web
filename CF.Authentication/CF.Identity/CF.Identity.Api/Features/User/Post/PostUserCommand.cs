using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Post;

public record PostUserCommand(EditableUserDto User) : IUnitRequest<Guid>, IRoleRequirement
{
    bool IRoleRequirement.Bypass => false;
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalWrite, UserRoles.UserWrite];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
