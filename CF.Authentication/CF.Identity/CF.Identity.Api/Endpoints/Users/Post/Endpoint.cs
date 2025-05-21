using CF.Identity.Api.Features.User;
using CF.Identity.Api.Features.User.Post;
using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions.Records;
using IDFCR.Shared.Http.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CF.Identity.Api.Endpoints.Users.Post;

public record PostRequest(Guid ClientId, string Username, string EmailAddress, string Password, string Firstname, string? Middlename, string Lastname, 
    string PrimaryTelephoneNumber, string PreferredUsername = null!) : MappableBase<IUser>, IEditableUser
{
    protected override IUser Source => new Features.User.UserDto
    {
        ClientId = ClientId,
        Username = Username,
        EmailAddress = EmailAddress,
        HashedPassword = Password,
        Firstname = Firstname,
        Middlename = Middlename,
        Lastname = Lastname,
        PrimaryTelephoneNumber = PrimaryTelephoneNumber,
        PreferredUsername = PreferredUsername
    };

    bool IUserSummary.IsSystem => false;
    string IEditableUser.HashedPassword => Password;
    Guid IDFCR.Shared.Abstractions.IIdentifer.Id => ClientId;

    public override void Map(IUser source)
    {
        throw new NotSupportedException();
    }
}

public static class Endpoint
{
    public static async Task<IResult> SaveUserAsync([FromForm] PostRequest request,
        IHttpContextAccessor httpContextAccessor,
        IMediator mediator, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new PostUserCommand(request.Map<EditableUserDto>()), cancellationToken);
        return result.NegotiateResult(httpContextAccessor, Endpoints.Url);
    }

    public static IEndpointRouteBuilder AddPostEndpoint(this IEndpointRouteBuilder builder)
    {
        builder.MapPost(Endpoints.Url, SaveUserAsync);
        return builder;
    }
}
