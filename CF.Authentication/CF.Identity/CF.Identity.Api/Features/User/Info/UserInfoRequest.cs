using CF.Identity.Api.Features.User.Get;
using CF.Identity.Infrastructure.Features.AccessToken;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.Identity.Api.Features.User.Info;

public record UserInfoRequest(IAccessToken AccessToken, Infrastructure.Features.Clients.IClientDetails Client) : IUnitRequest<UserInfoResponse>;

public class UserInfoRequestHandler(IMediator mediator) : IUnitRequestHandler<UserInfoRequest, UserInfoResponse>
{
    public async Task<IUnitResult<UserInfoResponse>> Handle(UserInfoRequest request, CancellationToken cancellationToken)
    {
        var usersResult = await mediator.Send(new FindUserByIdQuery(request.AccessToken!.UserId), cancellationToken);

        var user = usersResult.GetResultOrDefault();

        if (user is not null)
        {
            return UnitResult.FromResult(new UserInfoResponse(user.Id.ToString(), user.FormatName(),
                user.PreferredUsername ?? user.Username, user.EmailAddress), UnitAction.Get);
        }

        return new UnitResult(new UnauthorizedAccessException()).As<UserInfoResponse>();
    }
}