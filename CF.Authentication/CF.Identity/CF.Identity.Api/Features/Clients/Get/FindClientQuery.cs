using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Clients.Get;

public record FindClientQuery(string? Key = null, DateTimeOffset? ValidFrom = null, DateTimeOffset? ValidTo = null, bool ShowAll = false, 
    bool NoTracking = true, bool Bypass = false) 
    : IUnitRequestCollection<ClientDetailResponse>, IClientFilter, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => RoleRegistrar.List<ClientRoles>(RoleCategory.Read);
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
