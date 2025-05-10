using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Clients.Get;

public record FindClientQueryById(Guid Id) : IUnitRequest<ClientDetailResponse>;
