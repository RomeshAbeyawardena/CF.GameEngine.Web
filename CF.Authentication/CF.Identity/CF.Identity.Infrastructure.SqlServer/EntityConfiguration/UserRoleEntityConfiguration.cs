using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CF.Identity.Infrastructure.SqlServer.EntityConfiguration;

internal class UserRoleEntityConfiguration : IEntityTypeConfiguration<DbUserAccessRole>
{
    public void Configure(EntityTypeBuilder<DbUserAccessRole> builder)
    {
        builder.ToTable("UserRole", "dbo");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.UserId, x.AccessRoleId });
        builder.Property(x => x.Id)
            .HasColumnName("UserRoleId").ValueGeneratedOnAdd();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.AccessRoleId).IsRequired();

        builder.HasOne(x => x.AccessRole)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(x => x.AccessRoleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
