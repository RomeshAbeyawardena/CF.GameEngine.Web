using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CF.Identity.Infrastructure.SqlServer.EntityConfiguration;

internal class UserEntityConfiguration : IEntityTypeConfiguration<DbUser>
{
    public void Configure(EntityTypeBuilder<DbUser> builder)
    {
        builder.ToTable("User", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("UserId").IsRequired();
        builder.Property(x => x.EmailAddress).IsRequired().HasMaxLength(255);
        builder.Property(x => x.HashedPassword).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.Username).IsRequired().HasMaxLength(80);
        builder.Property(x => x.Firstname).IsRequired().HasMaxLength(32);
        builder.Property(x => x.MiddleName).HasMaxLength(32);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(32);
        builder.Property(x => x.ClientId).HasColumnName("ClientId");
        builder.Property(x => x.PreferredUsername).HasMaxLength(80);
        builder.Property(x => x.IsSystem).HasDefaultValue(false);
        builder.Property(x => x.RowVersion).IsConcurrencyToken().HasMaxLength(2000).IsRequired();
        builder.Property(x => x.Metadata).HasMaxLength(2000);
        builder.HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
}
