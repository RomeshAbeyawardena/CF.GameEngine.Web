using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CF.Identity.Infrastructure.SqlServer.EntityConfiguration;

internal class AccessTokenEntityConfiguration : IEntityTypeConfiguration<DbAccessToken>
{
    public void Configure(EntityTypeBuilder<DbAccessToken> builder)
    {
        builder.ToTable("AccessToken", "dbo");
        builder.HasKey(x => x.Id);
    }
}
