using System.Diagnostics;
using System.IO.Abstractions;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using Ignore;

namespace CSharpier.Cli;

internal class IgnoreWithBasePath(string basePath)
{
    public string BasePath => basePath;
    public readonly List<IgnoreRule> Rules = new();

    public bool TryIsIgnored(string path, out bool isIgnored)
    {
        isIgnored = false;
        var pathRelativeToIgnoreFile = path.Replace('\\', '/');
        if (!pathRelativeToIgnoreFile.StartsWith(basePath, StringComparison.Ordinal))
        {
            return false;
        }

        pathRelativeToIgnoreFile =
            pathRelativeToIgnoreFile.Length > basePath.Length
                ? pathRelativeToIgnoreFile[(basePath.Length + 1)..]
                : string.Empty;

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
        return hasMatchingRule;
    }

    public void Add(string rule)
    {
        this.Rules.Add(new IgnoreRule(rule));
    }
}

internal class IgnoreFile
{
    protected List<IgnoreWithBasePath> ignores { get; }
    private static readonly string[] alwaysIgnored = ["**/node_modules", "**/obj", "**/.git"];

    protected IgnoreFile(List<IgnoreWithBasePath> ignores)
    {
        this.ignores = ignores;
    }

    public bool IsIgnored(string filePath)
    {
        foreach (var ignore in this.ignores)
        {
            if (ignore.TryIsIgnored(filePath, out var result))
            {
                DebugLogger.Log(ignore);
                DebugLogger.Log($"{filePath} {result}");
                return result;
            }
        }

        return false;
    }

    public static async Task<IgnoreFile> CreateAsync(
        string baseDirectoryPath,
        IFileSystem fileSystem,
        CancellationToken cancellationToken
    )
    {
        var ignoreFilePaths = FindIgnorePath(baseDirectoryPath, fileSystem);
        if (ignoreFilePaths.Count == 0)
        {
            var ignore = new IgnoreWithBasePath(baseDirectoryPath.Replace('\\', '/'));
            foreach (var name in alwaysIgnored)
            {
                ignore.Add(name);
            }
            return new IgnoreFile([ignore]);
        }

        var ignores = new List<IgnoreWithBasePath>();
        foreach (var ignoreFilePath in ignoreFilePaths)
        {
            var ignore = new IgnoreWithBasePath(
                Path.GetDirectoryName(ignoreFilePath)!.Replace('\\', '/')
            );
            ignores.Insert(0, ignore);
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

    // this will return the ignore paths with the priority path coming last, which means the rules from the last entry will take
    // priority. .csharpierignore comes last, gitignore come before
    private static List<string> FindIgnorePath(string baseDirectoryPath, IFileSystem fileSystem)
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
                result.Insert(0, gitIgnoreFilePath);
            }

            directoryInfo = directoryInfo.Parent;
        }

        if (foundCSharpierIgnoreFilePath is not null)
        {
            result.Add(foundCSharpierIgnoreFilePath);
        }

        return result;
    }
}

internal class InvalidIgnoreFileException(string message, Exception exception)
    : Exception(message, exception);
