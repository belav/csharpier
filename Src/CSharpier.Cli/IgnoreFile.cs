using System.IO.Abstractions;
using System.Text.RegularExpressions;
using CSharpier.Core;

namespace CSharpier.Cli;

internal class IgnoreFile
{
    private List<IgnoreWithBasePath> Ignores { get; }
    private static readonly string alwaysIgnoredText = """
        **/node_modules
        **/obj
        **/.git
        """;

    private IgnoreFile(List<IgnoreWithBasePath> ignores)
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
        CancellationToken cancellationToken
    )
    {
        return await SharedFunc<IgnoreFile?>
            .GetOrAddAsync(
                baseDirectoryPath,
                async () =>
                {
                    DebugLogger.Log("Creating ignore file for " + baseDirectoryPath);
                    var ignoreFilePaths = FindIgnorePaths(baseDirectoryPath, fileSystem);
                    if (ignoreFilePaths.Count == 0)
                    {
                        var ignore = new IgnoreWithBasePath(baseDirectoryPath);
                        AddDefaultRules(ignore);
                        return new IgnoreFile([ignore]);
                    }

                    var ignores = new List<IgnoreWithBasePath>();
                    foreach (var ignoreFilePath in ignoreFilePaths)
                    {
                        var ignore = new IgnoreWithBasePath(Path.GetDirectoryName(ignoreFilePath)!);
                        AddDefaultRules(ignore);

                        var content = await fileSystem.File.ReadAllTextAsync(
                            ignoreFilePath,
                            cancellationToken
                        );

                        var (positives, negatives) = GitignoreParserNet.GitignoreParser.Parse(
                            content,
                            true
                        );

                        ignore.AddPositives(positives.Merged);
                        ignore.AddNegatives(negatives.Merged);

                        ignores.Add(ignore);
                    }

                    return new IgnoreFile(ignores);
                },
                cancellationToken
            )
            .ConfigureAwait(false);
    }

    private static void AddDefaultRules(IgnoreWithBasePath ignore)
    {
        var (alwaysPositives, alwaysNegatives) = GitignoreParserNet.GitignoreParser.Parse(
            alwaysIgnoredText,
            true
        );

        ignore.AddPositives(alwaysPositives.Merged);
        ignore.AddNegatives(alwaysNegatives.Merged);
    }

    // this will return the ignore paths in order of priority
    // the first csharpierignore it finds at or above the path
    // and then all .gitignores (at or above) it finds in order from closest to further away
    private static List<string> FindIgnorePaths(string baseDirectoryPath, IFileSystem fileSystem)
    {
        var result = new List<string>();
        string? foundCSharpierIgnoreFilePath = null;
        var directoryInfo = fileSystem.DirectoryInfo.New(baseDirectoryPath);
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

            var gitIgnoreFilePath = fileSystem.Path.Combine(directoryInfo.FullName, ".gitignore");
            if (fileSystem.File.Exists(gitIgnoreFilePath))
            {
                result.Add(gitIgnoreFilePath);
            }

            directoryInfo = directoryInfo.Parent;
        }

        if (foundCSharpierIgnoreFilePath is not null)
        {
            result.Insert(0, foundCSharpierIgnoreFilePath);
        }

        return result;
    }

    // modified from the nuget library to include the directory
    // that the ignore file exists at
    // and to return if this ignore file has a rule for a given path
    private class IgnoreWithBasePath(string basePath)
    {
        private readonly List<Regex> positives = [];
        private readonly List<Regex> negatives = [];

        public (bool hasMatchingRule, bool isIgnored) IsIgnored(string path)
        {
            if (!path.StartsWith(basePath, StringComparison.Ordinal))
            {
                return (false, false);
            }

            var pathRelativeToIgnoreFile =
                path.Length > basePath.Length
                    ? path[basePath.Length..].Replace('\\', '/')
                    : string.Empty;

            var isIgnored = false;
            var hasMatchingRule = false;
            foreach (var rule in this.positives)
            {
                var isMatch = rule.IsMatch(pathRelativeToIgnoreFile);
                if (isMatch)
                {
                    hasMatchingRule = true;
                }

                if (!isIgnored && isMatch)
                {
                    isIgnored = true;
                }
            }

            foreach (var rule in this.negatives)
            {
                var isMatch = rule.IsMatch(pathRelativeToIgnoreFile);
                if (isMatch)
                {
                    hasMatchingRule = true;
                }

                if (isIgnored && isMatch)
                {
                    isIgnored = false;
                }
            }

            return (hasMatchingRule, isIgnored);
        }

        public void AddPositives(Regex rule)
        {
            this.positives.Add(rule);
        }

        public void AddNegatives(Regex rule)
        {
            this.negatives.Add(rule);
        }

        public override string ToString()
        {
            return "BasePath = " + basePath;
        }
    }
}

internal class InvalidIgnoreFileException(string message, Exception exception)
    : Exception(message, exception);
