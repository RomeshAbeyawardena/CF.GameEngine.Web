using CF.GameEngine.Infrastructure.Features.Elements;
using IDFCR.Shared.Abstractions;

namespace CF.GameEngine.Infrastructure.SqlServer.Models;

public class Element : MappableBase<IElement>, IElement
{
    protected override IElement Source => this;
    public string? ExternalReference { get; set; }
    public string? Description { get; set; }
    public string Key { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int? SortOrder { get; set; }
    public Guid ElementTypeId { get; set; }
    public Guid ParentElementId { get; set; }
    public Guid Id { get; set; }

    public override void Map(IElement source)
    {
        ExternalReference = source.ExternalReference;
        Key = source.Key;
        Name = source.Name;
        Description = source.Description;
        Id = source.Id;
        SortOrder = source.SortOrder;
        ElementTypeId = source.ElementTypeId;
        ParentElementId = source.ParentElementId;
    }

    public virtual ElementType ElementType { get; set; } = null!;
}
