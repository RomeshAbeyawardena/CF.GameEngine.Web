using CF.Identity.Infrastructure.SqlServer.Models;
using CF.Identity.Infrastructure.SqlServer.PII;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Repositories;
using IDFCR.Shared.Abstractions.Results;

namespace CF.Identity.Infrastructure.SqlServer.Repositories;

public interface ICommonName : IMappable<ICommonName>, IIdentifer
{
    string Value { get; }
    string ValueNormalised { get; }
    string RowVersion { get; }
    string MetaData { get; }
}

internal class CommonNameDto : MappableBase<ICommonName>, ICommonName
{
    protected override ICommonName Source => this;

    public Guid Id { get; set; }
    public string Value { get; set; } = null!;
    public string ValueNormalised { get; set; } = null!;
    public string RowVersion { get; set; } = null!;
    public string MetaData { get; set; } = null!;

    public override void Map(ICommonName source)
    {
        Id = source.Id;
        Value = source.Value;
        ValueNormalised = source.ValueNormalised;
        RowVersion = source.RowVersion;
        MetaData = source.MetaData;
    }
}

internal interface ICommonNameRepository : IRepository<CommonNameDto>
{
    Task<IUnitResult<CommonNameDto>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}

internal class CommonNameRepository(ICommonNamePIIProtection commonNamePIIProtection, TimeProvider timeProvider, CFIdentityDbContext context) 
    : RepositoryBase<ICommonName, DbCommonName, CommonNameDto>(timeProvider, context)
{
    protected override void OnAdd(DbCommonName db, CommonNameDto source)
    {
        commonNamePIIProtection.Protect(db);
        base.OnAdd(db, source);
    }
}

