using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Clients.Get;

public record FindClientByIdQuery(Guid ClientId, bool Bypass = false) 
    : RoleRequirementBase(() => RoleRegistrar.List<ClientRoles>(RoleCategory.Read)), 
    IUnitRequest<ClientDetailResponse>
{
    
}
