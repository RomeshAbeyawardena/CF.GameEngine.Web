using CF.GameEngine.Infrastructure.Features.Elements;
using CF.GameEngine.Infrastructure.SqlServer.Models;
using IDFCR.Shared.Abstractions;
using LinqKit;

namespace CF.GameEngine.Infrastructure.SqlServer.Filters;

internal class ElementFilter(IElementFilter filter) :
    FilterBase<IElementFilter, Element>(filter), IElementFilter
{
    protected override IElementFilter Source => this;
    public Guid? ParentElementId { get; set; }
    public string? ExternalReference { get; set; }
    public string? Key { get; set; }
    public string? NameContains { get; set; }

    public override ExpressionStarter<Element> ApplyFilter(ExpressionStarter<Element> query, IElementFilter filter)
    {
        if (filter.ParentElementId.HasValue)
        {
            query = query.And(x => x.ParentElementId == filter.ParentElementId);
        }
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

    public override void Map(IElementFilter source)
    {
        throw new NotImplementedException();
    }
}
