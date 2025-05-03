using CF.GameEngine.Infrastructure.Features.ElementTypes;
using IDFCR.Shared.Abstractions.Records;

namespace CF.GameEngine.Web.Api.Features.ElementTypes;

public record ElementTypeResponseDetail : MappableBase<IElementType>, IElementTypeDetails
{
    protected override IElementType Source => 
        new Infrastructure.Features.ElementTypes.ElementTypeDto
        {
            Key = Key,
            Name = Name,
            ExternalReference = ExternalReference,
            Description = Description
        };

    public string Key { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string ExternalReference { get; set; } = null!;
    public string? Description { get; set; }
    public Guid Id { get; set; };

    public override void Map(IElementType source)
    {
        ExternalReference = source.ExternalReference;
        Key = source.Key;
        Name = source.Name;
        Description = source.Description;
        Id = source.Id;
    }
}
