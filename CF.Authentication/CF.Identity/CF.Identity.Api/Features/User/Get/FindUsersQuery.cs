using CF.Identity.Infrastructure.Features.Users;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.User.Get;

public record FindUsersQuery(Guid ClientId, string? Username = null, string? NameContains = null,
    bool? IsSystem = null, bool NoTracking = true, bool Bypass = false) 
    : RoleRequirementBase(() => RoleRegistrar.List<UserRoles>(RoleCategory.Read)), IUnitRequestCollection<UserDto>, IUserFilter
{
    
}
