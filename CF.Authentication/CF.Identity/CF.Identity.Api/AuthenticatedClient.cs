using CF.Identity.Infrastructure.Features.Clients;

namespace CF.Identity.Api;

public record AuthenticatedClient(string ClientId, IClientDetails ClientDetails);