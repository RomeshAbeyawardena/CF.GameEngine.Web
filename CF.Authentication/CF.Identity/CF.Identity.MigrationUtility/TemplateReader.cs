using Microsoft.Extensions.Primitives;

namespace CF.Identity.MigrationUtility;

public class Section
{
    public string? Name { get; set; }
    public Dictionary<string, StringValues> Content { get; } = [];
    public static void Append(IDictionary<string, StringValues> target, string key, string value)
    {
        var trimmedValue = value.Trim();
        if (!target.TryAdd(key, trimmedValue))
        {
            var collection = target[key].ToList();
            collection.Add(trimmedValue);
            target[key] = collection.ToArray();
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
