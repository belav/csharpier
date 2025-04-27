using System.IO.Abstractions;
using System.Text.RegularExpressions;
using CSharpier.Core;
using IniParser;
using IniParser.Model.Configuration;
using IniParser.Parser;

namespace CSharpier.Cli.EditorConfig;

internal static class EditorConfigFileParser
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

    public static EditorConfigFile Parse(string filePath, IFileSystem fileSystem)
    {
        // TODO parallel it seems we re-read this file, should it also use the semaphore?
        DebugLogger.Log("Reading file at " + filePath);
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

        return new EditorConfigFile
        {
            IsRoot = configData?.Global["root"] == "true",
            Sections = sections,
        };
    }
}
