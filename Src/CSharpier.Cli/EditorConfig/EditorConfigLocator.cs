using System.IO.Abstractions;
using CSharpier.Core;

namespace CSharpier.Cli.EditorConfig;

internal static class EditorConfigLocator
{
    public static async Task<EditorConfigSections?> FindForDirectoryNameAsync(
        string directoryName,
        IFileSystem fileSystem,
        IgnoreFile ignoreFile,
        CancellationToken cancellationToken
    )
    {
        if (directoryName is "")
        {
            return null;
        }

        return await SharedFunc<EditorConfigSections?>
            .GetOrAddAsync(
                directoryName,
#pragma warning disable CS1998
                async () =>
#pragma warning restore CS1998
                {
                    var directoryInfo = fileSystem.DirectoryInfo.New(directoryName);

                    while (directoryInfo is not null)
                    {
                        var file = fileSystem.FileInfo.New(
                            fileSystem.Path.Combine(directoryInfo.FullName, ".editorconfig")
                        );
                        if (file.Exists)
                        {
                            var dirName = fileSystem.Path.GetDirectoryName(file.FullName);
                            ArgumentNullException.ThrowIfNull(dirName);

                            return new EditorConfigSections
                            {
                                DirectoryName = dirName,
                                SectionsIncludingParentFiles = FindSections(
                                    file.FullName,
                                    fileSystem
                                ),
                            };
                        }

                        directoryInfo = directoryInfo.Parent;
                    }

                    return null;
                },
                cancellationToken
            )
            .ConfigureAwait(false);
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
