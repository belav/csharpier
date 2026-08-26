using System.Collections.Concurrent;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using CSharpier.Core;
using IniParser;
using IniParser.Model.Configuration;
using IniParser.Parser;

namespace CSharpier.Cli.EditorConfig;

internal static partial class EditorConfigFileParser
{
    // According to https://spec.editorconfig.org/#file-format
    // "Comment: starts with a ; or a #."
    [GeneratedRegex("^[;#].*$")]
    private static partial Regex CommentRegex();

    private static readonly IniParserConfiguration Configuration = new()
    {
        CommentRegex = CommentRegex(),
        AllowDuplicateKeys = true,
        AllowDuplicateSections = true,
        OverrideDuplicateKeys = true,
        SkipInvalidLines = true,
        ThrowExceptionsOnError = false,
    };

    // a nested .editorconfig is parsed along with every one of its ancestors, and each of those
    // ancestors is in turn an ancestor of the next nested file, so without this the same files get
    // reparsed O(depth) times. the write stamp is part of the key so that a config edited while a
    // server is running is still picked up
    private static readonly ConcurrentDictionary<
        (string Path, DateTime LastWriteTimeUtc, long Length),
        EditorConfigFile
    > ParsedFilesByPath = new();

    public static EditorConfigFile Parse(string filePath, IFileSystem fileSystem)
    {
        var fileInfo = fileSystem.FileInfo.New(filePath);

        return ParsedFilesByPath.GetOrAdd(
            (filePath, fileInfo.LastWriteTimeUtc, fileInfo.Length),
            static (key, fs) => ParseFile(key.Path, fs),
            fileSystem
        );
    }

    private static EditorConfigFile ParseFile(string filePath, IFileSystem fileSystem)
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

        return new EditorConfigFile
        {
            IsRoot = configData?.Global["root"] == "true",
            Sections = sections,
        };
    }
}
