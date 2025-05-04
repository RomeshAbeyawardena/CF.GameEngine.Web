using MediatR;

namespace CF.Identity.Api.Features.Authenticate;

public record RefreshExistingTokenCommand(string AuthToken) : IRequest<AuthenticateResponse>;
