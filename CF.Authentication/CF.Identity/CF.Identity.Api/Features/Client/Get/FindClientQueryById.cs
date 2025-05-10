using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Client.Get;

public record FindClientQueryById(Guid Id) : IUnitRequest<ClientDetailResponse>;
