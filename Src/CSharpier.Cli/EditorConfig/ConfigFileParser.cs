using System.IO.Abstractions;
using System.Text.RegularExpressions;
using IniParser;
using IniParser.Model.Configuration;
using IniParser.Parser;

namespace CSharpier.Cli.EditorConfig;

internal static class ConfigFileParser
{
    // According to https://spec.editorconfig.org/#file-format
    // "Comment: starts with a ; or a #."
    private static readonly Regex CommentRegex = new("^[;#].*$");

    private static readonly IniParserConfiguration Configuration = new()
    {
        CommentRegex = CommentRegex,
        AllowDuplicateKeys = true,
        AllowDuplicateSections = true,
        OverrideDuplicateKeys = true,
        SkipInvalidLines = true,
        ThrowExceptionsOnError = false,
    };

    public static ConfigFile Parse(string filePath, IFileSystem fileSystem)
    {
        var directory = fileSystem.Path.GetDirectoryName(filePath);

        ArgumentNullException.ThrowIfNull(directory);

        var parser = new FileIniDataParser(new IniDataParser(Configuration));

        using var stream = fileSystem.File.OpenRead(filePath);
        using var streamReader = new StreamReader(stream);
        var configData = parser.ReadData(streamReader);

        var sections = new List<Section>();
        if (configData is not null)
        {
            sections.AddRange(configData.Sections.Select(s => new Section(s, directory)));
        }

        return new ConfigFile
        {
            IsRoot = configData?.Global["root"] == "true",
            Sections = sections,
        };
    }
}
