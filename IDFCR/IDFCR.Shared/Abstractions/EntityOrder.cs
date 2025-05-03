namespace IDFCR.Shared.Abstractions;

public enum Sort { Ascending, Descending }

public interface IEntityOrder
{
    string? SortField { get; }
    string? SortOrder { get; }
}

public class EntityOrder : MappableBase<IEntityOrder>, IEntityOrder
{
    private static string? SortOrderToString(Sort? sort)
    {
        return sort switch
        {
            Sort.Ascending => "asc",
            Sort.Descending => "desc",
            _ => null
        };
    }

    public EntityOrder(IEntityOrder entityOrder, string defaultSortField, Sort defaultSortOrder = Sort.Ascending)
    {
        Map(entityOrder);
        SortField = string.IsNullOrWhiteSpace(SortField) ? defaultSortField : SortField;
        SortOrder = string.IsNullOrWhiteSpace(SortOrder) ? SortOrderToString(defaultSortOrder) : SortOrder;
    }

    private Sort? _sort;
    protected override EntityOrder Source => this;
    public string? SortField { get; set; } = null!;
    public string? SortOrder { get; set; } = null!;
    public Sort? Order { 
        get => _sort; 
        set
        {
            _sort = value;
            SortOrder = SortOrderToString(_sort);
        }
    }

    public override void Map(IEntityOrder source)
    {
        SortField = source.SortField;
        SortOrder = source.SortOrder;
    }

    public override string ToString()
    {
        return $"{SortField} {SortOrder}";
    }
}