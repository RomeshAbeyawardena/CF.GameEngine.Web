using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions.Records;

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
        PreferredUsername = PreferredUsername ?? Username
    };

    bool IUserSummary.IsSystem => false;
    string IEditableUser.HashedPassword => Password;
    Guid IDFCR.Shared.Abstractions.IIdentifer.Id => Guid.Empty;

    public override void Map(IUser source)
    {
        //this is the top level mapping method, so we don't need to do anything here
        throw new NotSupportedException();
    }
}
