// based on the code at https://github.com/markashleybell/MAB.DotIgnore
// simplified to remove unneeded features and fixed a couple of edgecases that were not handled correctly

using System.Collections.Concurrent;
using System.IO.Abstractions;

namespace CSharpier.Cli.DotIgnore;

internal class IgnoreList(string basePath)
{
    private static readonly string[] alwaysIgnoredText =
    [
        "**/bin",
        "**/node_modules",
        "**/obj",
        "**/.git",
    ];
    private readonly List<IgnoreRule> rules = [];

    public static async Task<IgnoreList> CreateAsync(
        IFileSystem fileSystem,
        string basePath,
        string? ignoreFilePath,
        CancellationToken cancellationToken
    )
    {
        var ignoreList = new IgnoreList(basePath);
        ignoreList.AddRules(
            alwaysIgnoredText.Concat(
                ignoreFilePath is null
                    ? Enumerable.Empty<string>()
                    : await fileSystem.File.ReadAllLinesAsync(ignoreFilePath, cancellationToken)
            )
        );
        return ignoreList;
    }

    private void AddRules(IEnumerable<string> newRules)
    {
        this.rules.AddRange(
            newRules
                .Select(o => o.Trim())
                .Where(o => o.Length > 0 && !o.StartsWith('#'))
                .Select(o => new IgnoreRule(o))
        );
    }

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

        var ancestorIgnored = this.IsAnyParentDirectoryIgnored(pathRelativeToIgnoreFile);

        if (ancestorIgnored)
        {
            return (true, true);
        }

        return this.IsPathIgnored(pathRelativeToIgnoreFile, false);
    }

    private bool IsAnyParentDirectoryIgnored(string path)
    {
        var nextPathIndex = path.LastIndexOf('/');
        if (nextPathIndex > 0)
        {
            return this.IsDirectoryIgnored(path[..nextPathIndex]);
        }

        return false;
    }

    private readonly ConcurrentDictionary<string, bool> directoryIgnoredByPath = new();

    private bool IsDirectoryIgnored(string path)
    {
        if (this.directoryIgnoredByPath.TryGetValue(path, out var isIgnored))
        {
            return isIgnored;
        }

        if (this.IsPathIgnored(path, true) is (true, true))
        {
            isIgnored = true;
        }

        if (!isIgnored)
        {
            var nextPathIndex = path.LastIndexOf('/');
            if (nextPathIndex > 0)
            {
                isIgnored = this.IsDirectoryIgnored(path[..nextPathIndex]);
            }
        }

        this.directoryIgnoredByPath.TryAdd(path, isIgnored);
        return isIgnored;
    }

    private (bool hasMatchingRule, bool isIgnored) IsPathIgnored(string path, bool pathIsDirectory)
    {
        // This pattern modified from https://github.com/henon/GitSharp/blob/master/GitSharp/IgnoreRules.cs
        var isIgnored = false;
        var hasMatchingRule = false;

        foreach (var rule in this.rules)
        {
            var isNegativeRule = (rule.PatternFlags & PatternFlags.NEGATION) != 0;

            if (
                (!isIgnored && isNegativeRule || isIgnored == isNegativeRule)
                && rule.IsMatch(path, pathIsDirectory)
            )
            {
                hasMatchingRule = true;
                isIgnored = !isNegativeRule;
            }
        }

        return (hasMatchingRule, isIgnored);
    }
}
