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

public record ClientApiKey(Guid Id, Guid ClientId, string ApiKey, string Name, DateTimeOffset ValidFrom, DateTimeOffset ValidTo);
public record Client(Guid Id, string Name, DateTimeOffset ValidFrom, DateTimeOffset ValidTo)
{
    public virtual ICollection<ClientApiKey> ClientApiKeys { get; set; } = [];
}

public interface IClientFilter
{
    public string? ApiKey { get; }
}

public record ClientFilter(string? ApiKey) : IClientFilter;


public interface IClientRepository
{
    Task<Client?> FindClientAsync(IClientFilter clientFilter, CancellationToken cancellationToken);
}

public class FindClientCommandHandler(IClientRepository clientRepository) : IRequestHandler<FindClientCommand, Client?>
{
    public async Task<Client?> Handle(FindClientCommand request, CancellationToken cancellationToken)
    {
        return await clientRepository.FindClientAsync(request.ClientFilter, cancellationToken);
    }
}

public record User(Guid Id, string Username, string Secret);

public interface IUserFilter
{
    public string? Username { get; }
}

public record UserFilter(string? Username) : IUserFilter;

public record FindUserCommand(IUserFilter ClientFilter) : IRequest<User?>
{
}

public interface IUserRepository
{
    Task<User?> FindUserAsync(IUserFilter userFilter, CancellationToken cancellationToken);
}

public class FindUserCommandHandler(IUserRepository userRepository) : IRequestHandler<FindUserCommand, User?>
{
    public async Task<User?> Handle(FindUserCommand request, CancellationToken cancellationToken)
    {
        return await userRepository.FindUserAsync(request.ClientFilter, cancellationToken);
    }
}