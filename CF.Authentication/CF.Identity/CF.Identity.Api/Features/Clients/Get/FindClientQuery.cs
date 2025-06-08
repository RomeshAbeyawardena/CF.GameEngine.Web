using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Clients.Get;

public record FindClientQuery(string? Key = null, DateTimeOffset? ValidFrom = null, DateTimeOffset? ValidTo = null, bool ShowAll = false,
    bool NoTracking = true, bool Bypass = false)
    : RoleRequirementBase(() => RoleRegistrar.List<ClientRoles>(RoleCategory.Read)), IUnitRequestCollection<ClientDetailResponse>, IClientFilter;