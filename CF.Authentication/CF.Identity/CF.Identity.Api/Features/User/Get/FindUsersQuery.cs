using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Get;

public record FindUsersQuery(Guid ClientId, string? Username = null, string? NameContains = null,
    bool? IsSystem = null, bool NoTracking = true, bool Bypass = false) : IUnitRequestCollection<UserDto>, IUserFilter, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalRead, UserRoles.UserRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
