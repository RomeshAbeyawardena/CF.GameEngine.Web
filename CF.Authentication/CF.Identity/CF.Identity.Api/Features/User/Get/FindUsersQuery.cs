using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Get;

public record FindUsersQuery(Guid ClientId, string? Username = null, string? NameContains = null, 
    bool? IsSystem = null, bool NoTracking = true, bool Bypass = false) : IUnitRequestCollection<UserDto>, IUserFilter, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [Roles.GlobalRead, Roles.UserRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
