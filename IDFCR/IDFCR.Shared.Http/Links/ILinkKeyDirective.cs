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
        ILinkKeyDirectiveOptions Options { get; }
        string SimplifyRel(string value);
    ///other methods for externally managing links
    }


/// <summary>
/// The default <see cref="ILinkKeyDirective"/> implementation."/>
/// </summary>
internal class DefaultLinkKeyDirective(ILinkKeyDirectiveOptions options) : ILinkKeyDirective
{
    public ILinkKeyDirectiveOptions Options => options;
    public string SimplifyRel(string value)
    {
        var newKey = value;
        foreach (var item in options.Affixes)
        {
            newKey = newKey.Replace(item, string.Empty, Options.StringComparison
                .GetValueOrDefault(StringComparison.InvariantCultureIgnoreCase));
        }

        return newKey;
    }
}
