using System.IO.Abstractions;

namespace CSharpier.Cli.EditorConfig;

internal static class EditorConfigParser
{
    public static List<EditorConfigSections> GetAllForDirectory(
        string directoryName,
        IFileSystem fileSystem
    )
    {
        if (directoryName is "")
        {
            return new List<EditorConfigSections>();
        }

        var editorConfigFiles = fileSystem.DirectoryInfo
            .FromDirectoryName(directoryName)
            .EnumerateFiles(".editorconfig", SearchOption.AllDirectories);

        return editorConfigFiles
            .Select(
                o =>
                    new EditorConfigSections
                    {
                        DirectoryName = fileSystem.Path.GetDirectoryName(o.FullName),
                        SectionsIncludingParentFiles = FindSections(o.FullName, fileSystem)
                    }
            )
            .OrderBy(o => o.DirectoryName)
            .ToList();
    }

    private static List<Section> FindSections(string filePath, IFileSystem fileSystem)
    {
        var editorConfigFiles = ParseConfigFiles(
                fileSystem.Path.GetDirectoryName(filePath),
                fileSystem
            )
            .Reverse()
            .ToList();
        return editorConfigFiles.SelectMany(configFile => configFile.Sections).ToList();
    }

    private static IEnumerable<ConfigFile> ParseConfigFiles(
        string directoryPath,
        IFileSystem fileSystem
    )
    {
        var directory = fileSystem.DirectoryInfo.FromDirectoryName(directoryPath);
        while (directory != null)
        {
            var potentialPath = fileSystem.Path.Combine(directory.FullName, ".editorconfig");
            if (fileSystem.File.Exists(potentialPath))
            {
                var configFile = ConfigFileParser.Parse(potentialPath, fileSystem);

                DebugLogger.Log(potentialPath);
                yield return configFile;
                if (configFile.IsRoot)
                {
                    yield break;
                }
            }

            directory = directory.Parent;
        }
    }
}