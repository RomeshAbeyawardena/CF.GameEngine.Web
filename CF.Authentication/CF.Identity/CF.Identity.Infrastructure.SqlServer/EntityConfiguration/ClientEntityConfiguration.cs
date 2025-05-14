using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CF.Identity.Infrastructure.SqlServer.EntityConfiguration;

internal class ClientEntityConfiguration : IEntityTypeConfiguration<DbClient>
{
    public void Configure(EntityTypeBuilder<DbClient> builder)
    {
        builder.ToTable("Client", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().HasColumnName("ClientId").IsRequired();
        builder.Property(x => x.Reference).IsRequired().HasMaxLength(80);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(120);
        builder.Property(x => x.DisplayName).HasMaxLength(250);
        builder.Property(x => x.ValidFrom).IsRequired().HasColumnType("datetimeoffset(7)");
        builder.Property(x => x.ValidTo).HasColumnType("datetimeoffset(7)");
        builder.Property(x => x.SuspendedTimestampUtc).HasColumnType("datetimeoffset(7)");
        builder.Property(x => x.SecretHash).HasMaxLength(2000);
        builder.Property(x => x.IsSystem).HasDefaultValue(false);
    }
}
