using System.IO.Abstractions;

namespace CSharpier.Cli.EditorConfig;

internal static class EditorConfigParser
{
    public static List<EditorConfigSections> FindForDirectoryName(
        string directoryName,
        IFileSystem fileSystem,
        IgnoreFile ignoreFile
    )
    {
        if (directoryName is "")
        {
            return [];
        }

        var directoryInfo = fileSystem.DirectoryInfo.New(directoryName);
        var editorConfigFiles = directoryInfo
            .EnumerateFiles(".editorconfig", SearchOption.TopDirectoryOnly)
            .Where(x => !ignoreFile.IsIgnored(x.FullName))
            .ToList();

        // need to also search the parent directories for any above the given directory
        directoryInfo = directoryInfo.Parent;
        while (directoryInfo is not null)
        {
            var file = fileSystem.FileInfo.New(
                fileSystem.Path.Combine(directoryInfo.FullName, ".editorconfig")
            );
            if (file.Exists)
            {
                editorConfigFiles.Add(file);
            }

            directoryInfo = directoryInfo.Parent;
        }

        return editorConfigFiles
            .Select(o =>
            {
                var dirName = fileSystem.Path.GetDirectoryName(o.FullName);
                ArgumentNullException.ThrowIfNull(dirName);

                return new EditorConfigSections
                {
                    DirectoryName = dirName,
                    SectionsIncludingParentFiles = FindSections(o.FullName, fileSystem),
                };
            })
            .OrderByDescending(o => o.DirectoryName.Length)
            .ToList();
    }

    private static List<Section> FindSections(string filePath, IFileSystem fileSystem)
    {
        return ParseConfigFiles(fileSystem.Path.GetDirectoryName(filePath)!, fileSystem)
            .Reverse()
            .SelectMany(configFile => configFile.Sections)
            .ToList();
    }

    private static IEnumerable<ConfigFile> ParseConfigFiles(
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
                var configFile = CSharpierConfigParser.Parse(potentialPath, fileSystem);

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
