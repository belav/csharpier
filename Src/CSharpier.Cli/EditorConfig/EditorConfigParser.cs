using System.IO.Abstractions;

namespace CSharpier.Cli.EditorConfig;

// TODO for a given directory find the config in it and going up til root
// keep track of directories + their corresponding configs
// if searching a new directory and we go up to a parent that contains the first config, use that
// how many directories would we run into? enough to matter?
internal static class EditorConfigParser
{
    /// <summary>Finds all configs above the given directory as well as within the subtree of this directory</summary>
    public static List<EditorConfigSections> FindForDirectoryName(
        string directoryName,
        IFileSystem fileSystem,
        // not the best name, but I plan on rewriting this to not find all of the configs up front
        // which will remove this parameter
        bool limitEditorConfigSearch,
        IgnoreFile ignoreFile
    )
    {
        if (directoryName is "")
        {
            return new List<EditorConfigSections>();
        }

        var directoryInfo = fileSystem.DirectoryInfo.New(directoryName);
        var editorConfigFiles = directoryInfo
            .EnumerateFiles(
                ".editorconfig",
                limitEditorConfigSearch
                    ? SearchOption.TopDirectoryOnly
                    : SearchOption.AllDirectories
            )
            .Where(x => !ignoreFile.IsIgnored(x.FullName))
            .ToList();

        // already found any in this directory above
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
                var configFile = ConfigFileParser.Parse(potentialPath, fileSystem);

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
