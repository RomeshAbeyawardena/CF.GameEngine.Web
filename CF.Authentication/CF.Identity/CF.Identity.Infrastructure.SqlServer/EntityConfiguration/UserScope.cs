using CF.Identity.Infrastructure.SqlServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CF.Identity.Infrastructure.SqlServer.EntityConfiguration;

internal class UserScope : IEntityTypeConfiguration<DbUserScope>
{
    public void Configure(EntityTypeBuilder<DbUserScope> builder)
    {
        throw new NotImplementedException();
    }
}
