using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Models;

public class DbUser : MappableBase<IUser>, IUser
{
    protected override IUser Source => this;
    public string EmailAddress { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Firstname { get; set; } = null!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = null!;
    public Guid ClientId { get; set; }
    public string? PreferredUsername { get; set; }
    public Guid Id { get; set; }

    public override void Map(IUser source)
    {
        EmailAddress = source.EmailAddress;
        HashedPassword = source.HashedPassword;
        Username = source.Username;
        Firstname = source.Firstname;
        MiddleName = source.MiddleName;
        LastName = source.LastName;
        ClientId = source.ClientId;
        PreferredUsername = source.PreferredUsername;
        Id = source.Id;
    }
}
