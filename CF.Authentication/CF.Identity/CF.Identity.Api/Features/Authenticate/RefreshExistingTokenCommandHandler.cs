using MediatR;

namespace CF.Identity.Api.Features.Authenticate;

public class RefreshExistingTokenCommandHandler : IRequestHandler<RefreshExistingTokenCommand, AuthenticateResponse>
{
    public Task<AuthenticateResponse> Handle(RefreshExistingTokenCommand request, CancellationToken cancellationToken)
    {
        // Simulate token refresh logic
        if(request.AuthToken == "valid-refresh-token")
        {
            // Generate a new token and refresh token
            return Task.FromResult(new AuthenticateResponse(true, "new-token", "new-refresh-token", DateTimeOffset.UtcNow.AddHours(1), []));
        }

        return Task.FromResult(new AuthenticateResponse(false, null, null, null, []));
    }
}
