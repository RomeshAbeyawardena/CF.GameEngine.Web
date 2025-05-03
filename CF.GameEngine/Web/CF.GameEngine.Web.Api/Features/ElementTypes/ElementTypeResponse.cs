using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Records;

namespace CF.GameEngine.Web.Api.Features.ElementTypes;

public record ElementTypeResponse : MappableBase<IElementType>, IElementTypeSummary
{
    protected override IElementType Source => 
        new ElementTypeDto
        {
            Key = Key,
            Name = Name
        };

    public string Key { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int? SortOrder { get; set; }
    public Guid Id { get; set; }

    public override void Map(IElementType source)
    {
        SortOrder = source.SortOrder;
        Id = source.Id;
        Key = source.Key;
        Name = source.Name;
    }
}
