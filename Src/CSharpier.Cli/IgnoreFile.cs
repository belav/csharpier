using System.IO.Abstractions;
using Ignore;

namespace CSharpier.Cli;

internal class IgnoreFile
{
    private List<IgnoreWithBasePath> ignores { get; }
    private static readonly string[] alwaysIgnored = ["**/node_modules", "**/obj", "**/.git"];

    private IgnoreFile(List<IgnoreWithBasePath> ignores)
    {
        this.ignores = ignores;
    }

    public bool IsIgnored(string filePath)
    {
        foreach (var ignore in this.ignores)
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
        var ignoreFilePaths = FindIgnorePaths(baseDirectoryPath, fileSystem);
        if (ignoreFilePaths.Count == 0)
        {
            var ignore = new IgnoreWithBasePath(baseDirectoryPath);
            foreach (var name in alwaysIgnored)
            {
                ignore.Add(name);
            }
            return new IgnoreFile([ignore]);
        }

        var ignores = new List<IgnoreWithBasePath>();
        foreach (var ignoreFilePath in ignoreFilePaths)
        {
            var ignore = new IgnoreWithBasePath(Path.GetDirectoryName(ignoreFilePath)!);
            ignores.Add(ignore);
            foreach (var name in alwaysIgnored)
            {
                ignore.Add(name);
            }
            foreach (
                var line in await fileSystem.File.ReadAllLinesAsync(
                    ignoreFilePath,
                    cancellationToken
                )
            )
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    ignore.Add(line);
                }
                catch (Exception ex)
                {
                    throw new InvalidIgnoreFileException(
                        @$"The .csharpierignore file at {ignoreFilePath} could not be parsed due to the following line:
{line}
",
                        ex
                    );
                }
            }
        }

        return new IgnoreFile(ignores);
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
        private readonly List<IgnoreRule> Rules = new();

        public (bool hasMatchingRule, bool isIgnored) IsIgnored(string path)
        {
            if (!path.StartsWith(basePath, StringComparison.Ordinal))
            {
                return (false, false);
            }

            var pathRelativeToIgnoreFile =
                path.Length > basePath.Length
                    ? path[(basePath.Length + 1)..].Replace('\\', '/')
                    : string.Empty;

            var isIgnored = false;
            var hasMatchingRule = false;
            foreach (var rule in this.Rules)
            {
                var isMatch = rule.IsMatch(pathRelativeToIgnoreFile);
                if (isMatch)
                {
                    hasMatchingRule = true;
                }
                if (rule.Negate)
                {
                    if (isIgnored && isMatch)
                    {
                        isIgnored = false;
                    }
                }
                else if (!isIgnored && isMatch)
                {
                    isIgnored = true;
                }
            }
            return (hasMatchingRule, isIgnored);
        }

        public void Add(string rule)
        {
            this.Rules.Add(new IgnoreRule(rule));
        }
    }
}

internal class InvalidIgnoreFileException(string message, Exception exception)
    : Exception(message, exception);
