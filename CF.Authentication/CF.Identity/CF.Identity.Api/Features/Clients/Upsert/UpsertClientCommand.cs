using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Clients.Upsert;

public record UpsertClientCommand(EditableClientDto Client, bool Bypass = false)
    : RoleRequirementBase(() => RoleRegistrar.List<ClientRoles>(RoleCategory.Write)), IUnitRequest<Guid>;

public class UpsertClientCommandHandler(IClientRepository clientRepository) : IUnitRequestHandler<UpsertClientCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(UpsertClientCommand request, CancellationToken cancellationToken)
    {
        return await clientRepository
            .UpsertAsync(request.Client.Map<Infrastructure.Features.Clients.ClientDto>(), cancellationToken);
    }
}