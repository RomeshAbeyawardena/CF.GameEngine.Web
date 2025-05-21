using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.User.Post;

public record PostUserCommand(EditableUserDto User) : IUnitRequest<Guid>;

public class PostUserCommandHandler(IUserRepository userRepository) : IUnitRequestHandler<PostUserCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(PostUserCommand request, CancellationToken cancellationToken)
    { 
        return await userRepository.UpsertAsync(request.User.Map<Infrastructure.Features.Users.UserDto>(), cancellationToken);
    }
}
