namespace IDFCR.Shared.Abstractions.Filters;

public interface IValidity
{
    DateTimeOffset ValidFrom { get; }
    DateTimeOffset? ValidTo { get; }
}
