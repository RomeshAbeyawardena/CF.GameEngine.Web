using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace CF.Identity.Infrastructure.SqlServer.Models;

public class DbUser : MappableBase<IUser>, IUser
{
    protected override IUser Source => this;
    string IUserDetail.Firstname => FirstCommonName.ValueNormalised;
    string? IUserDetail.MiddleName => MiddleCommonName?.ValueNormalised;
    string IUserDetail.LastName => LastCommonName.ValueNormalised;

    public string EmailAddress { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public string Username { get; set; } = null!;

    public Guid FirstCommonNameId { get; set; }
    public Guid? MiddleCommonNameId { get; set; }
    public Guid LastCommonNameId { get; set; }

    public Guid ClientId { get; set; }
    public string? PreferredUsername { get; set; }
    public Guid Id { get; set; }
    public bool IsSystem { get; set; }
    public string RowVersion { get; set; } = null!;

    public virtual DbClient Client { get; set; } = null!;
    public virtual DbCommonName FirstCommonName { get; set; } = null!;
    public virtual DbCommonName MiddleCommonName { get; set; } = null!;
    public virtual DbCommonName LastCommonName { get; set; } = null!;

    public string? Metadata { get; set; }

    public override void Map(IUser source)
    {
        EmailAddress = source.EmailAddress;
        Username = source.Username;
        HashedPassword = source.HashedPassword;
        ClientId = source.ClientId;
        PreferredUsername = source.PreferredUsername;
        Id = source.Id;
        IsSystem = source.IsSystem;
        RowVersion = source.RowVersion;
        Metadata = source.Metadata;
    }
}
