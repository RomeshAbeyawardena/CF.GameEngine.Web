using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CF.Identity.Infrastructure.SqlServer.EntityConfiguration;

internal class RoleEntityConfiguration : IEntityTypeConfiguration<DbAccessRole>
{
    public void Configure(EntityTypeBuilder<DbAccessRole> builder)
    {
        builder.ToTable("Role", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("RoleId");
        builder.Property(x => x.Key)
            .IsRequired().HasMaxLength(64);
        builder.Property(x => x.DisplayName)
            .HasMaxLength(200);
        builder.HasIndex(x => new { x.ClientId, x.Key });

        builder.HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.UserRoles)
            .WithOne(x => x.AccessRole)
            .HasForeignKey(x => x.AccessRoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Scopes)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.AccessRoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
