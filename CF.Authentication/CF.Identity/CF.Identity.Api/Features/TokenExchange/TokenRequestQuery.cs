using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.TokenExchange;

public record TokenRequestQuery(TokenRequest TokenRequest) : IUnitRequest<TokenResponse>;
