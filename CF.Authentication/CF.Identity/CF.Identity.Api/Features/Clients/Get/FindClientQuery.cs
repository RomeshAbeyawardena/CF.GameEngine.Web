using CF.Identity.Infrastructure.Features.Clients;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Clients.Get;

public record FindClientQuery(string? Key = null, DateTimeOffset? ValidFrom = null, DateTimeOffset? ValidTo = null,  bool NoTracking = true) 
    : IUnitRequestCollection<ClientDetailResponse>, IClientFilter;
