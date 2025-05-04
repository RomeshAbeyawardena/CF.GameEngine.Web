using MediatR;

namespace CF.Identity.Api.Features.Authenticate;

public record AuthenticateCommand(string ApiKey, string Username, string Secret) : IRequest<AuthenticateResponse>;
public class AuthenticateCommandHandler(IMediator mediator, TimeProvider timeProvider) : IRequestHandler<AuthenticateCommand, AuthenticateResponse>
{
    public async Task<AuthenticateResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
        // Simulate authentication logic

        //an api-key belonging to a given client
        var client = await mediator.Send(new FindClientCommand(new ClientFilter(request.ApiKey)), cancellationToken);

        if(client is null)
        {
            return new AuthenticateResponse(false, null, null, null, []);
        }

        //a username of a valid user and a secret they only know, we only maintain a hash of this secret
        var user = await mediator.Send(new FindUserCommand(new UserFilter(request.Username)), cancellationToken);

        if (user is null || user.Secret != request.Secret)
        {
            return new AuthenticateResponse(false, null, null, null, []);
        }

        //TODO: Create a JWT token and pass to user, let's pretend this was what was spat out by the command responsible for this...
        var generatedToken = "xyz";
        var refreshToken = "abc";
        var validTo = timeProvider.GetUtcNow().AddHours(4);
        return new AuthenticateResponse(true, generatedToken, refreshToken, null, []);
    }
}
