namespace CF.Identity.Integration;

internal interface INode
{
    string Key { get; }
    int? NodeIndex { get; }
    bool? Required { get; }
    IRoot? Root { get; }
    IEnumerable<IElement> Elements { get; }
    bool CanHaveMultipleChildren { get; }
    string ValidationFormat { get; }
    bool DeclaresExpectation { get; }
}

internal interface IRoot
{
    string Type { get; }
    string Name { get; }
}

internal interface IElement : IRoot
{
    bool? Required { get; }
}