using MediatR;

namespace CF.Identity.Api.Features.Authenticate;

public record AuthenticateCommand(string ApiKey, string Username, string Secret) : IRequest<AuthenticateResponse>;
public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponse>
{
    public Task<AuthenticateResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        // Simulate authentication logic
        if (request.ApiKey == "valid-api-key" && request.Username == "user" && request.Secret == "password")
        {
            return Task.FromResult(new AuthenticateResponse(true, "token", "refresh-token", DateTimeOffset.UtcNow.AddHours(1), []));
        }
        return Task.FromResult(new AuthenticateResponse(false, null, null, null, []));
    }
}

public record RefreshExistingTokenCommand(string AuthToken) : IRequest<AuthenticateResponse>;

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
