using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Clients.Get;

public record FindClientQueryById(Guid Id, bool Bypass = false) : IUnitRequest<ClientDetailResponse>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [Roles.GlobalRead, Roles.ClientRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}

