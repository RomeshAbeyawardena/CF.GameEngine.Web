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
        builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("AcessTokenId").IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.ReferenceToken).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.RefreshToken).HasMaxLength(2000);
        builder.Property(x => x.AccessToken).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.ClientId).IsRequired();
        builder.Property(x => x.Type).IsRequired().HasMaxLength(120);
        builder.Property(x => x.ValidFrom).IsRequired().HasColumnType("datetimeoffset(7)");
        builder.Property(x => x.ValidTo).HasColumnType("datetimeoffset(7)");
        builder.HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
