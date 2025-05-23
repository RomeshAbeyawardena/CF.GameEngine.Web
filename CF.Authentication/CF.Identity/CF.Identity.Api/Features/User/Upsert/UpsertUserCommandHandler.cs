using CF.Identity.Api.Features.User.Assign;
using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.Identity.Api.Features.User.Upsert;

public class UpsertUserCommandHandler(IMediator mediator, IUserRepository userRepository) : IUnitRequestHandler<UpsertUserCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(UpsertUserCommand request, CancellationToken cancellationToken)
    {
        var userToUpsert = request.User.Map<Infrastructure.Features.Users.UserDto>();
        var upsertedUserId = await userRepository.UpsertAsync(userToUpsert, cancellationToken);

        var userId = upsertedUserId.GetResultOrDefault();

        if (string.IsNullOrWhiteSpace(request.User.Scope))
        {
            return upsertedUserId;
        }

        var scopes = request.User.Scope.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var scopeResult = await mediator.Send(new AssignUserScopesCommand(userToUpsert.ClientId, userId, scopes), cancellationToken);

        if (scopeResult.IsSuccess)
        {
            return upsertedUserId;
        }

        return scopeResult.As<Guid>();
    }
}
