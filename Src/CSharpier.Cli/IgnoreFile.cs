using System.IO.Abstractions;
using CSharpier.Cli.DotIgnore;
using CSharpier.Core;

namespace CSharpier.Cli;

internal class IgnoreFile
{
    private List<IgnoreList> Ignores { get; }

    private IgnoreFile(List<IgnoreList> ignores)
    {
        this.Ignores = ignores;
    }

    public bool IsIgnored(string filePath)
    {
        filePath = filePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

        foreach (var ignore in this.Ignores)
        {
            // when using one of the ignore files to determine if a given file is ignored or not
            // we can only consider that file if it actually has a matching rule for the filePath
            var (hasMatchingRule, isIgnored) = ignore.IsIgnored(filePath);
            if (hasMatchingRule)
            {
                return isIgnored;
            }
        }

        return false;
    }

    public static async Task<IgnoreFile?> CreateAsync(
        string baseDirectoryPath,
        IFileSystem fileSystem,
        string? ignorePath,
        CancellationToken cancellationToken
    )
    {
        Task<IgnoreList> CreateIgnore(string ignoreFilePath, string? overrideBasePath)
        {
            return IgnoreList.CreateAsync(
                fileSystem,
                overrideBasePath ?? Path.GetDirectoryName(ignoreFilePath)!,
                ignoreFilePath,
                cancellationToken
            );
        }

        return await SharedFunc<IgnoreFile?>
            .GetOrAddAsync(
                baseDirectoryPath,
                async () =>
                {
                    DebugLogger.Log("Creating ignore file for " + baseDirectoryPath);
                    if (ignorePath is not null)
                    {
                        if (!fileSystem.File.Exists(ignorePath))
                        {
                            throw new Exception("There was no ignore file found at " + ignorePath);
                        }

                        DebugLogger.Log("Using ignorePath: " + ignorePath);
                        return new IgnoreFile([await CreateIgnore(ignorePath, baseDirectoryPath)]);
                    }
                    else
                    {
                        var ignoreFilePaths = FindIgnorePaths(baseDirectoryPath, fileSystem);
                        if (ignoreFilePaths.Count == 0)
                        {
                            return new IgnoreFile([
                                await IgnoreList.CreateAsync(
                                    fileSystem,
                                    baseDirectoryPath,
                                    null,
                                    cancellationToken
                                ),
                            ]);
                        }

                        var ignores = new List<IgnoreList>();
                        foreach (var ignoreFilePath in ignoreFilePaths)
                        {
                            ignores.Add(await CreateIgnore(ignoreFilePath, null));
                        }

                        return new IgnoreFile(ignores);
                    }
                },
                cancellationToken
            )
            .ConfigureAwait(false);
    }

    // this will return the ignore paths in order of priority
    // the first csharpierignore it finds at or above the path
    // and then all .gitignores (at or above stopping once it encounters a .git directory) it finds in order from closest to further away
    private static List<string> FindIgnorePaths(string baseDirectoryPath, IFileSystem fileSystem)
    {
        var result = new List<string>();
        string? foundCSharpierIgnoreFilePath = null;
        var directoryInfo = fileSystem.DirectoryInfo.New(baseDirectoryPath);
        var includeGitIgnores = true;
        while (directoryInfo != null)
        {
            if (foundCSharpierIgnoreFilePath is null)
            {
                var csharpierIgnoreFilePath = fileSystem.Path.Combine(
                    directoryInfo.FullName,
                    ".csharpierignore"
                );
                if (fileSystem.File.Exists(csharpierIgnoreFilePath))
                {
                    foundCSharpierIgnoreFilePath = csharpierIgnoreFilePath;
                }
            }

            if (includeGitIgnores)
            {
                var gitIgnoreFilePath = fileSystem.Path.Combine(
                    directoryInfo.FullName,
                    ".gitignore"
                );
                if (fileSystem.File.Exists(gitIgnoreFilePath))
                {
                    result.Add(gitIgnoreFilePath);
                }
            }

            if (fileSystem.Directory.Exists(Path.Combine(directoryInfo.FullName, ".git")))
            {
                includeGitIgnores = false;
            }

            directoryInfo = directoryInfo.Parent;
        }

        if (foundCSharpierIgnoreFilePath is not null)
        {
            result.Insert(0, foundCSharpierIgnoreFilePath);
        }

        return result;
    }
}

internal class InvalidIgnoreFileException(string message, Exception exception)
    : Exception(message, exception);
