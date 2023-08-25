using System.IO.Abstractions;
using IniParser;

namespace CSharpier.Cli.EditorConfig;

internal static class ConfigFileParser
{
    public static ConfigFile Parse(string filePath, IFileSystem fileSystem)
    {
        var parser = new FileIniDataParser();

        using var stream = fileSystem.File.OpenRead(filePath);
        using var streamReader = new StreamReader(stream);
        var configData = parser.ReadData(streamReader);

        var directory = fileSystem.Path.GetDirectoryName(filePath);
        var sections = new List<Section>();
        foreach (var section in configData.Sections)
        {
            sections.Add(new Section(section, directory));
        }
        return new ConfigFile { IsRoot = configData.Global["root"] == "true", Sections = sections };
    }
}
