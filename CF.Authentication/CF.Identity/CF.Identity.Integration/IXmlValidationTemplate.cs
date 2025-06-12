using System.Diagnostics;

namespace CF.Identity.Integration;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
internal record XmlValidationTemplate(string XmlPath, string XsdPath) : IXmlValidationTemplate
{
    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}

internal interface IXmlValidationTemplate
{
    public string XmlPath { get; }
    public string XsdPath { get; }
}