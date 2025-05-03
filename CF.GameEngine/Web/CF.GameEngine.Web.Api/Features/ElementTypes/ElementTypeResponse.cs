using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Records;

namespace CF.GameEngine.Web.Api.Features.ElementTypes;

public record ElementTypeResponse : MappableBase<IElementType>, IElementTypeSummary
{
    protected override IElementType Source => 
        new Infrastructure.Features.ElementTypes.ElementTypeDto
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
        Key = source.Key;
        Name = source.Name;
    }
}
