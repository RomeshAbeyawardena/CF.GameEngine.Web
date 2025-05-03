using IDFCR.Shared.Abstractions;

namespace CF.GameEngine.Infrastructure.Features.Elements;

public class ElementDto : MappableBase<IElement>, IElement
{
    public string? ExternalReference { get; set; }
    public string? Description { get; set; }
    public string Key { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int? SortOrder { get; set; }
    public Guid ElementTypeId { get; set; }
    public Guid ParentElementId { get; set; }
    public Guid Id { get; set; }

    protected override IElement Source => this;
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
}
