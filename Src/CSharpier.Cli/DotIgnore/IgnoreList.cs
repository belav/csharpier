// based on the code at https://github.com/markashleybell/MAB.DotIgnore
// simplified to remove unneeded features and fixed a couple of edgecases that were not handled correctly

using System.Collections.Concurrent;
using System.IO.Abstractions;

namespace CSharpier.Cli.DotIgnore;

internal class IgnoreList(string basePath)
{
    private static readonly IgnoreRule[] alwaysIgnoredRules =
    [
        new("**/bin"),
        new("**/node_modules"),
        new("**/obj"),
        new("**/.git"),
    ];
    private IgnoreRule[] rules = [];

    public static async Task<IgnoreList> CreateAsync(
        IFileSystem fileSystem,
        string basePath,
        string? ignoreFilePath,
        CancellationToken cancellationToken
    )
    {
        var ignoreList = new IgnoreList(basePath);
        ignoreList.AddRules(
            ignoreFilePath is null
                ? Enumerable.Empty<string>()
                : await fileSystem.File.ReadAllLinesAsync(ignoreFilePath, cancellationToken)
        );
        return ignoreList;
    }

    private void AddRules(IEnumerable<string> newRules)
    {
        // TODO it seems like we have two rules that are both "*", they most likely have different pattern flags to account for the ! and the /
        this.rules = newRules
            .Select(o => o.Trim())
            .Where(o => o.Length > 0 && !o.StartsWith('#'))
            .Select(o => new IgnoreRule(o))
            .ToArray();
    }

    public (bool hasMatchingRule, bool isIgnored) IsIgnored(string path, bool isDirectory)
    {
        if (!path.StartsWith(basePath, StringComparison.Ordinal))
        {
            return (false, false);
        }

        var pathRelativeToIgnoreFile =
            path.Length > basePath.Length ? PathRelativeToIgnoreFile(path) : string.Empty;

        var ancestorIgnored = this.IsAnyParentDirectoryIgnored(pathRelativeToIgnoreFile);

        if (ancestorIgnored)
        {
            return (true, true);
        }

        return this.IsPathIgnored(pathRelativeToIgnoreFile, isDirectory);
    }

    private ReadOnlySpan<char> PathRelativeToIgnoreFile(string path)
    {
        var relativeSpan = path.AsSpan()[basePath.Length..];
        var index = relativeSpan.IndexOf('\\');
        return index < 0 ? relativeSpan : path[basePath.Length..].Replace('\\', '/');
    }

    private bool IsAnyParentDirectoryIgnored(ReadOnlySpan<char> path)
    {
        var nextPathIndex = path.LastIndexOf('/');
        if (nextPathIndex > 0)
        {
            return this.IsDirectoryIgnored(path[..nextPathIndex]);
        }

        return false;
    }

    private readonly ConcurrentDictionary<string, bool> directoryIgnoredByPath = new();

    private bool IsDirectoryIgnored(ReadOnlySpan<char> path)
    {
        var lookup = this.directoryIgnoredByPath.GetAlternateLookup<ReadOnlySpan<char>>();
        if (lookup.TryGetValue(path, out var isIgnored))
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

        lookup.TryAdd(path, isIgnored);
        return isIgnored;
    }

    private (bool hasMatchingRule, bool isIgnored) IsPathIgnored(
        ReadOnlySpan<char> path,
        bool pathIsDirectory
    )
    {
        // This pattern modified from https://github.com/henon/GitSharp/blob/master/GitSharp/IgnoreRules.cs
        var isIgnored = false;
        var hasMatchingRule = false;

        EvaluateRules(
            alwaysIgnoredRules,
            path,
            pathIsDirectory,
            ref isIgnored,
            ref hasMatchingRule
        );
        EvaluateRules(this.rules, path, pathIsDirectory, ref isIgnored, ref hasMatchingRule);

        return (hasMatchingRule, isIgnored);
    }

    private static void EvaluateRules(
        IgnoreRule[] rules,
        ReadOnlySpan<char> path,
        bool pathIsDirectory,
        ref bool isIgnored,
        ref bool hasMatchingRule
    )
    {
        foreach (var rule in rules)
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
    }
}
