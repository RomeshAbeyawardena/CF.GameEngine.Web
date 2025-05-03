using IDFCR.Shared.Abstractions;

namespace CF.GameEngine.Infrastructure.Features.ElementTypes;

public interface IElementTypeFilter : IFilter<IElementType>
{
    string? ExternalReference { get; }
    string? Key { get; }
    string? NameContains { get; }
    
}
