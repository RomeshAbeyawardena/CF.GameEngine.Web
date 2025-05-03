namespace IDFCR.Shared.Abstractions;

public interface IValidityFilter
{
    DateTimeOffset? ValidFrom { get; }
    DateTimeOffset? ValidTo { get; }
}