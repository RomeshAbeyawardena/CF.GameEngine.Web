using CF.Identity.Infrastructure.Features;
using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Clients.Get;


public record FindClientByIdQuery(Guid ClientId, bool Bypass = false) : IUnitRequest<ClientDetailResponse>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [SystemRoles.GlobalRead, ClientRoles.ClientRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}
