using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Client.Get;

public record FindClientByIdQuery(Guid ClientId) : IUnitRequest<ClientDetailResponse>;
