using IDFCR.Shared.Abstractions.Results;
using MediatR;

namespace CF.Identity.Api.Features.Client.Get;

public record FindClientQuery(Guid ClientId) : IRequest<IUnitResult<ClientDetailResponse>>;
