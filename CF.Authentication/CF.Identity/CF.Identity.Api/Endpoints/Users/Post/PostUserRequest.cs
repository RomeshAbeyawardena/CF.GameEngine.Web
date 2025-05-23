using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions.Records;

namespace CF.Identity.Api.Endpoints.Users.Post;

public record PostUserRequest(string Username, string EmailAddress, string Password, string Firstname, string? Middlename, string Lastname, 
    string PrimaryTelephoneNumber, string? PreferredUsername = null) : MappableBase<IUser>
{
    public string? Client { get; set; } 
    public Guid? ClientId { get; set; }
    public string? Scope { get; set; }

    protected override IUser Source => new Features.User.EditableUserDto
    {
        ClientId = ClientId.GetValueOrDefault(),
        Client = Client,
        Username = Username,
        EmailAddress = EmailAddress,
        HashedPassword = Password,
        Firstname = Firstname,
        Middlename = Middlename,
        Lastname = Lastname,
        PrimaryTelephoneNumber = PrimaryTelephoneNumber,
        PreferredUsername = PreferredUsername ?? Username,
        Scope = Scope
    };


    public override void Map(IUser source)
    {
        //this is the top level mapping method, so we don't need to do anything here
        throw new NotSupportedException();
    }
}
