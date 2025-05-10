using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessToken;

public record UpsertAccessTokenCommand(AccessTokenDto AccessToken) : IUnitRequest<Guid>;
