using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CF.Identity.Infrastructure.SqlServer.EntityConfiguration;

internal class CommonNameEntityConfiguration : IEntityTypeConfiguration<DbCommonName>
{
    public void Configure(EntityTypeBuilder<DbCommonName> builder)
    {
        builder.ToTable("CommonName", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName("CommonNameId");

        builder.Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.ValueCI)
            .IsRequired()
            .HasMaxLength(344);

        builder.Property(x => x.ValueHmac)
            .HasMaxLength(2000);

        builder.Property(x => x.RowVersion)
            .IsConcurrencyToken()
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.MetaData)
            .IsRequired(false)
            .HasMaxLength(2000);
    }
}
