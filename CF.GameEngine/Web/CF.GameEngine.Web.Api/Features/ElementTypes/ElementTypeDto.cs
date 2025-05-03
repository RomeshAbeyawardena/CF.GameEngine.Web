using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Records;

namespace CF.GameEngine.Web.Api.Features.ElementTypes;

public record ElementTypeDto : MappableBase<IElementType>, IElementType
{
    protected override IElementType Source => this;

    public string? ExternalReference { get; set; } = null!;
    public string? Description { get; set; }
    public string Key { get; set; } = null!;
    public string Name { get; set; } = null!;
    public Guid Id { get; set; }
    public int? SortOrder { get; set; }

    public override void Map(IElementType source)
    {
        ExternalReference = source.ExternalReference;
        Key = source.Key;
        Name = source.Name;
        Description = source.Description;
        Id = source.Id;
        SortOrder = source.SortOrder;
    }
}
