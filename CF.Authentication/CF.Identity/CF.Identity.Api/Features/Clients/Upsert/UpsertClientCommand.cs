using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Clients.Upsert;

public record UpsertClientCommand(EditableClientDto Client, bool Bypass = false)
    : IUnitRequest<Guid>, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => RoleRegistrar.List<ClientRoles>(RoleCategory.Write);
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}

public class UpsertClientCommandHandler(IClientRepository clientRepository) : IUnitRequestHandler<UpsertClientCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(UpsertClientCommand request, CancellationToken cancellationToken)
    {
        return await clientRepository
            .UpsertAsync(request.Client.Map<Infrastructure.Features.Clients.ClientDto>(), cancellationToken);
    }
}