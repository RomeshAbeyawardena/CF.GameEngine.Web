namespace IDFCR.Shared.Http.Links;

public interface ILinkKeyDirectiveOptions
{
    IEnumerable<string> Affixes { get; }
    StringComparison? StringComparison { get; }
}

public record LinkKeyDirectiveOptions(IEnumerable<string> Affixes,
    StringComparison? StringComparison = StringComparison.InvariantCultureIgnoreCase)
    : ILinkKeyDirectiveOptions
{
    public static LinkKeyDirectiveOptions Default => new(["id", "guid", "ref", "identifier"], System.StringComparison.InvariantCultureIgnoreCase);
}

    public interface ILinkKeyDirective
{
    string SimplifyRel(string value, LinkKeyDirectiveOptions options);
    ///other methods for externally managing links
}


/// <summary>
/// The default <see cref="ILinkKeyDirective"/> implementation."/>
/// </summary>
internal class DefaultLinkKeyDirective : ILinkKeyDirective
{
    public string SimplifyRel(string value, LinkKeyDirectiveOptions options)
    {
        var newKey = value;
        foreach (var item in options.Affixes)
        {
            newKey = value.Replace(item, string.Empty, options.StringComparison
                .GetValueOrDefault(StringComparison.InvariantCultureIgnoreCase));
        }

        return newKey;
    }
}
