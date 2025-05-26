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
        
        builder.Property(x => x.EmailAddress).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.EmailAddressHmac).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.EmailAddressCI).IsRequired().HasMaxLength(344);
        builder.Property(x => x.HashedPassword).IsRequired().HasMaxLength(2000);
        
        builder.Property(x => x.Username).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.UsernameHmac).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.UsernameCI).IsRequired().HasMaxLength(344);

        builder.HasOne(x => x.FirstCommonName)
            .WithMany()
            .HasForeignKey(x => x.FirstCommonNameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.MiddleCommonName)
            .WithMany()
            .HasForeignKey(x => x.MiddleCommonNameId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.LastCommonName)
            .WithMany()
            .HasForeignKey(x => x.LastCommonNameId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.ClientId).HasColumnName("ClientId");
        
        builder.Property(x => x.PreferredUsername).HasMaxLength(2000);
        builder.Property(x => x.PreferredUsernameHmac).HasMaxLength(2000);
        builder.Property(x => x.PreferredUsernameCI).IsRequired().HasMaxLength(344);

        builder.Property(x => x.PrimaryTelephoneNumber).HasMaxLength(2000);
        builder.Property(x => x.PrimaryTelephoneNumberHmac).HasMaxLength(2000);

        builder.Property(x => x.IsSystem).HasDefaultValue(false);
        builder.Property(x => x.RowVersion).IsConcurrencyToken().HasMaxLength(2000).IsRequired();
        builder.Property(x => x.Metadata).HasMaxLength(2000);
        builder.HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder.HasMany(x => x.UserScopes)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
