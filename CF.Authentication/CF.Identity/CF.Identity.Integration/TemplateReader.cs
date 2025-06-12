using System.Xml;
using System.Xml.Schema;

namespace CF.Identity.Integration;

internal static class TemplateValidator
{
    public static void ValidateXml(IXmlValidationTemplate validationTemplate)
    {
        var settings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            DtdProcessing = DtdProcessing.Ignore,
            ValidationFlags =
                XmlSchemaValidationFlags.ReportValidationWarnings |
                XmlSchemaValidationFlags.ProcessIdentityConstraints |
                XmlSchemaValidationFlags.ProcessSchemaLocation
        };

        settings.Schemas.Add(null, validationTemplate.XsdPath);
        settings.ValidationEventHandler += (sender, args) =>
        {
            throw new XmlSchemaValidationException(
                $"Validation error: {args.Message} (Line {args.Exception?.LineNumber}, Position {args.Exception?.LinePosition})"
            );
        };

        using var reader = XmlReader.Create(validationTemplate.XmlPath, settings);
        while (reader.Read()) { } // Will throw if invalid
    }


    public static void ValidateTemplate(IXmlValidationTemplate template, IEnumerable<Section> sections)
    {
        ValidateXml(template);

    }
}

internal static class TemplateReader
{
    public static async Task<IEnumerable<Section>> ReadTemplate(string fileName)
    {
        if (!File.Exists(fileName))
        {
            throw new FileNotFoundException();
        }

        var sections = new List<Section>();

        using var reader = new StreamReader(fileName);
        Section? currentSection = null;
        while (!reader.EndOfStream) {
            var line = await reader.ReadLineAsync();
            if (line is null || line.StartsWith(';')) continue;

            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                if(currentSection is not null)
                {
                    sections.Add(currentSection);
                }

                currentSection = new Section { Name = line.Trim('[', ']') };
                continue;
            }

            if (line.Contains('=') && currentSection is not null)
            {
                var lineSplit = line.Split('=', 2);

                Section.Append(currentSection.Content,lineSplit[0], lineSplit[1]);
                continue;
            }
        }

        return sections;
    }
}
