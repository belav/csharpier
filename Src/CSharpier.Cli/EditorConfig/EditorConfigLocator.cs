using System.IO.Abstractions;

namespace CSharpier.Cli.EditorConfig;

internal static class EditorConfigLocator
{
    public static EditorConfigSections? FindForDirectoryName(
        string directoryName,
        IFileSystem fileSystem,
        IgnoreFile ignoreFile
    )
    {
        if (directoryName is "")
        {
            return null;
        }

        var directoryInfo = fileSystem.DirectoryInfo.New(directoryName);

        while (directoryInfo is not null)
        {
            var file = fileSystem.FileInfo.New(
                fileSystem.Path.Combine(directoryInfo.FullName, ".editorconfig")
            );
            if (file.Exists && !ignoreFile.IsIgnored(file.FullName))
            {
                var dirName = fileSystem.Path.GetDirectoryName(file.FullName);
                ArgumentNullException.ThrowIfNull(dirName);

                return new EditorConfigSections
                {
                    DirectoryName = dirName,
                    SectionsIncludingParentFiles = FindSections(file.FullName, fileSystem),
                };
            }

            directoryInfo = directoryInfo.Parent;
        }

        return null;
    }

    private static List<Section> FindSections(string filePath, IFileSystem fileSystem)
    {
        return ParseConfigFiles(fileSystem.Path.GetDirectoryName(filePath)!, fileSystem)
            .Reverse()
            .SelectMany(configFile => configFile.Sections)
            .ToList();
    }

    private static IEnumerable<EditorConfigFile> ParseConfigFiles(
        string directoryPath,
        IFileSystem fileSystem
    )
    {
        var directory = fileSystem.DirectoryInfo.New(directoryPath);
        while (directory != null)
        {
            var potentialPath = fileSystem.Path.Combine(directory.FullName, ".editorconfig");
            if (fileSystem.File.Exists(potentialPath))
            {
                var configFile = EditorConfigFileParser.Parse(potentialPath, fileSystem);

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
