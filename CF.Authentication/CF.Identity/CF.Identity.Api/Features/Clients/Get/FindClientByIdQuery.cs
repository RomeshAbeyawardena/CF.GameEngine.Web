using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Clients.Get;

public static class Roles
{
    public const string Client = "client:api:read";
}

public record FindClientByIdQuery(Guid ClientId, bool Bypass = false) : IUnitRequest<ClientDetailResponse>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [Roles.Client];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.All;
}
