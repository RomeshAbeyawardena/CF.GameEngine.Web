using CF.GameEngine.Infrastructure.Features.Elements;
using IDFCR.Shared.Abstractions.Records;

namespace CF.GameEngine.Web.Api.Features.Element;

public record ElementResponse : MappableBase<IElement>, IElementSummary
{
    protected override IElement Source => new ElementDto
    {
        Id = Id,
        Key = Key,
        Name = Name,
        ElementTypeId = ElementTypeId,
        ParentElementId = ParentElementId,
        SortOrder = SortOrder
    };

    public string Key { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int? SortOrder { get; set; }
    public Guid ElementTypeId { get; set; }
    public Guid ParentElementId { get; set; }
    public Guid Id { get; set; }

    public override void Map(IElement source)
    {
        Id = source.Id;
        Key = source.Key;
        Name = source.Name;
        ElementTypeId = source.ElementTypeId;
        ParentElementId = source.ParentElementId;
        SortOrder = source.SortOrder;
    }
}
