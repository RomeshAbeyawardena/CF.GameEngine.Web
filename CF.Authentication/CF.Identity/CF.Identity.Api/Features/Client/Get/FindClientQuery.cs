using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Client.Get;

public record FindClientQuery(string? Key, bool NoTracking = true) : IUnitRequestCollection<ClientDetailResponse>, IClientFilter;
