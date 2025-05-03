using CF.GameEngine.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CF.GameEngine.Infrastructure.SqlServer.Extensions.EntityConfiguration;

internal class ElementTypeEntityConfiguration : IEntityTypeConfiguration<ElementType>
{
    public void Configure(EntityTypeBuilder<ElementType> builder)
    {
        var entityName = nameof(ElementType);
        builder.ToTable(entityName);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasColumnName($"{entityName}Id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ExternalReference)
            .HasMaxLength(120);

        builder.Property(x => x.Key)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(80);

        builder.Property(x => x.Description)
            .HasMaxLength(250);
    }
}
