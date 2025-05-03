namespace IDFCR.Shared.Abstractions;

public interface IValidity
{
    DateTimeOffset ValidFrom { get; }
    DateTimeOffset? ValidTo { get; }
}
