namespace CF.GameEngine.Infrastructure.Features.Elements;

public interface IElementDetails : IElementSummary
{
    string? ExternalReference { get; }
    string? Description { get; }
}