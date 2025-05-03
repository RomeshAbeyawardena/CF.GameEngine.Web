using CF.GameEngine.Infrastructure.Features.ElementTypes;
using CF.GameEngine.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions;
using LinqKit;

namespace CF.GameEngine.Infrastructure.SqlServer.Filters;

internal class ElementTypeFilter(IElementTypeFilter filter) : FilterBase<IElementTypeFilter, ElementType>(filter), IElementTypeFilter
{
    protected override IElementTypeFilter Source => this;

    public string? ExternalReference { get; set; }
    public string? Key { get; set; }
    public string? NameContains { get; set; }

    public override ExpressionStarter<ElementType> ApplyFilter(ExpressionStarter<ElementType> query, IElementTypeFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.ExternalReference))
        {
            query = query.And(x => x.ExternalReference == filter.ExternalReference);
        }

        if (!string.IsNullOrWhiteSpace(filter.Key))
        {
            query = query.And(x => x.Key == filter.Key);
        }

        if (!string.IsNullOrWhiteSpace(filter.NameContains))
        {
            query = query.And(x => x.Name.Contains(filter.NameContains));
        }

        return base.ApplyFilter(query, filter);
    }

    public override void Map(IElementTypeFilter source)
    {
        ExternalReference = source.ExternalReference;
        Key = source.Key;
        NameContains = source.NameContains;
    }
}
