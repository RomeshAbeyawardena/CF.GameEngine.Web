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
    }
}
