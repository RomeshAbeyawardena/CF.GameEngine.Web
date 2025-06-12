namespace CF.Identity.Integration;

internal static class TemplateValidator
{
    public static void ValidateTemplate(IEnumerable<Section> sections)
    {
        if (sections is null || !sections.Any())
        {
            throw new ArgumentException("Template must contain at least one section.");
        }
        foreach (var section in sections)
        {
            if (string.IsNullOrWhiteSpace(section.Name))
            {
                throw new ArgumentException("Section name cannot be empty.");
            }
            if (section.Content is null || !section.Content.Any())
            {
                throw new ArgumentException($"Section '{section.Name}' must contain at least one key-value pair.");
            }
        }
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
