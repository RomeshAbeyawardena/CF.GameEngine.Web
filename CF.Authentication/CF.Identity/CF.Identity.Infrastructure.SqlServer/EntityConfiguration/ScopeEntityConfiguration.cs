using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CF.Identity.Infrastructure.SqlServer.EntityConfiguration;

internal class ScopeEntityConfiguration : IEntityTypeConfiguration<DbScope>
{
    public void Configure(EntityTypeBuilder<DbScope> builder)
    {
        builder.ToTable("Scope", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("ScopeId").IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(40);
        builder.Property(x => x.Description).HasMaxLength(250);
        builder.Property(x => x.Key).IsRequired().HasMaxLength(20);
        builder.Property(x => x.ClientId).HasColumnName("ClientId").IsRequired(false);
        builder.HasMany(x => x.UserScopes)
            .WithOne(x => x.Scope)
            .HasForeignKey(x => x.ScopeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Client)
            .WithMany()
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
