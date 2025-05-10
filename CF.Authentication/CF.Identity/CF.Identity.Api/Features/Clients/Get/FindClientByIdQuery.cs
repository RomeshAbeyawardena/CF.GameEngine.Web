using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Clients.Get;

public record FindClientByIdQuery(Guid ClientId) : IUnitRequest<ClientDetailResponse>;
