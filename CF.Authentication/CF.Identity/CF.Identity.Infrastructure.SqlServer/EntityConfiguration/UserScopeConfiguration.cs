using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CF.Identity.Infrastructure.SqlServer.EntityConfiguration;

internal class UserScopeConfiguration : IEntityTypeConfiguration<DbUserScope>
{
    public void Configure(EntityTypeBuilder<DbUserScope> builder)
    {
        builder.ToTable("UserScope", "dbo");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.UserId, x.ScopeId });
        builder.Property(x => x.Id)
            .HasColumnName("UserScopeId").ValueGeneratedOnAdd();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.ScopeId).IsRequired();
    }
}
