namespace IDFCR.Shared.Abstractions.Filters;

public interface IValidityFilter
{
    DateTimeOffset? ValidFrom { get; }
    DateTimeOffset? ValidTo { get; }
}