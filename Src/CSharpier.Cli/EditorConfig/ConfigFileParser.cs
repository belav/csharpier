using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace CSharpier.Cli.EditorConfig;

internal static class ConfigFileParser
{
    private static readonly Regex SectionRegex = new(@"^\s*\[(([^#;]|\\#|\\;)+)\]\s*([#;].*)?$");
    private static readonly Regex CommentRegex = new(@"^\s*[#;]");
    private static readonly Regex PropertyRegex =
        new(@"^\s*([\w\.\-_]+)\s*[=:]\s*(.*?)\s*([#;].*)?$");

    private static readonly HashSet<string> KnownProperties =
        new(
            new[] { "indent_style", "indent_size", "tab_width", "max_line_length", "root", },
            StringComparer.OrdinalIgnoreCase
        );

    public static ConfigFile Parse(string filePath, IFileSystem fileSystem)
    {
        var lines = fileSystem.File.ReadLines(filePath);

        var isRoot = false;
        var propertiesBySection = new Dictionary<string, Dictionary<string, string?>>();
        var sectionName = string.Empty;
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || CommentRegex.IsMatch(line))
            {
                continue;
            }

            var propertyMatches = PropertyRegex.Matches(line);
            if (propertyMatches.Count > 0)
            {
                var key = propertyMatches[0].Groups[1].Value.Trim().ToLowerInvariant();
                var value = propertyMatches[0].Groups[2].Value.Trim();

                if (KnownProperties.Contains(key))
                {
                    value = value.ToLowerInvariant();
                }

                if (sectionName is "")
                {
                    if (key == "root" && bool.TryParse(value, out var parsedValue))
                    {
                        isRoot = parsedValue;
                    }
                }
                else
                {
                    propertiesBySection[sectionName][key] = value;
                }
            }
            else
            {
                var sectionMatches = SectionRegex.Matches(line);
                if (sectionMatches.Count <= 0)
                {
                    continue;
                }

                sectionName = sectionMatches[0].Groups[1].Value;
                propertiesBySection[sectionName] = new Dictionary<string, string?>();
            }
        }

        var directory = fileSystem.Path.GetDirectoryName(filePath);
        return new ConfigFile
        {
            IsRoot = isRoot,
            Sections = propertiesBySection
                .Select(o => new Section(o.Key, directory, o.Value))
                .ToList()
        };
    }
}
