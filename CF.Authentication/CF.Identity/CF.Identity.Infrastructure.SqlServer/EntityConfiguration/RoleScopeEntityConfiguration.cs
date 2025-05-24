using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CF.Identity.Infrastructure.SqlServer.EntityConfiguration;

internal class RoleScopeEntityConfiguration : IEntityTypeConfiguration<DbRoleScope>
{
    public void Configure(EntityTypeBuilder<DbRoleScope> builder)
    {
        builder.ToTable("RoleScope", "dbo");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.RoleId, x.ScopeId });
        builder.Property(x => x.Id)
            .HasColumnName("RoleScopeId").ValueGeneratedOnAdd();
        builder.Property(x => x.RoleId).IsRequired();
        builder.Property(x => x.ScopeId).IsRequired();

        builder.HasOne(x => x.Role)
            .WithMany(x => x.Scopes)
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Scope)
            .WithMany()
            .HasForeignKey(x => x.ScopeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
