using CF.GameEngine.Infrastructure.Features.Elements;
using CF.GameEngine.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions;
using LinqKit;

namespace CF.GameEngine.Infrastructure.SqlServer.Filters;

internal class ElementFilter(IElementFilter filter) :
    FilterBase<IElementFilter, Element>(filter), IElementFilter
{
    protected override IElementFilter Source => this;
    public Guid? ParentElementId { get; } = filter.ParentElementId;
    public string? ExternalReference { get; } = filter.ExternalReference;
    public string? Key { get; } = filter.Key;
    public string? NameContains { get; } = filter.NameContains;

    public override ExpressionStarter<Element> ApplyFilter(ExpressionStarter<Element> query, IElementFilter filter)
    {
        if (filter.ParentElementId.HasValue)
        {
            query = query.And(x => x.ParentElementId == ParentElementId);
        }

        if (!string.IsNullOrWhiteSpace(ExternalReference))
        {
            query = query.And(x => x.ExternalReference == ExternalReference);
        }

        if (!string.IsNullOrWhiteSpace(Key))
        {
            query = query.And(x => x.Key == Key);
        }

        if (!string.IsNullOrWhiteSpace(NameContains))
        {
            query = query.And(x => x.Name.Contains(NameContains));
        }

        return base.ApplyFilter(query, filter);
    }

    public override void Map(IElementFilter source)
    {
        throw MappingNotSupportedException;
    }
}
