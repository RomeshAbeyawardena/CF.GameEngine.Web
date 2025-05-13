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
    }
}
