using System.IO.Abstractions;

namespace CSharpier.Cli.DotIgnore;

// TODO #1768 performance test this to see how it compares to the previous regex version
// TODO #1768 add link back to source repo
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
            ),
            MatchFlags.PATHNAME
        );
        return ignoreList;
    }

    private void AddRules(IEnumerable<string> rules, MatchFlags flags)
    {
        var ruleList = rules
            .Select(o => o.Trim())
            .Where(line => line.Length > 0 && !line.StartsWith('#'))
            .Select(o => new IgnoreRule(o, flags));

        this.rules.AddRange(ruleList);
    }

    public (bool hasMatchingRule, bool isIgnored) IsIgnored(
        string filePath,
        bool isDirectory = false
    )
    {
        if (!filePath.StartsWith(basePath, StringComparison.Ordinal))
        {
            return (false, false);
        }

        var pathRelativeToIgnoreFile =
            filePath.Length > basePath.Length
                ? filePath[basePath.Length..].Replace('\\', '/')
                : string.Empty;

        return this.IsIgnored2(pathRelativeToIgnoreFile, isDirectory);
    }

    private (bool hasMatchingRule, bool isIgnored) IsIgnored2(string path, bool pathIsDirectory)
    {
        // TODO #1768 this seems to have to run for almost every rule, why?
        var ancestorIgnored = this.IsAnyParentDirectoryIgnored(path);

        if (ancestorIgnored)
        {
            return (true, true);
        }

        return this.IsPathIgnored(path, pathIsDirectory);
    }

    // TODO #1768 assuming we keep this optimize the method because we will be looking up the same parent directories often
    private bool IsAnyParentDirectoryIgnored(string path)
    {
        var segments = path.NormalisePath()
            .Split('/')
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

        segments.RemoveAt(segments.Count - 1);

        var directory = new List<string>();

        // Loop over all the path segments (moving down the directory tree)
        // and test each as a directory, returning immediately if true
        foreach (var segment in segments)
        {
            directory.Add(segment);

            if (this.IsPathIgnored(string.Join("/", directory), true) is (true, true))
            {
                return true;
            }
        }

        return false;
    }

    private (bool hasMatchingRule, bool isIgnored) IsPathIgnored(string path, bool pathIsDirectory)
    {
        // This pattern modified from https://github.com/henon/GitSharp/blob/master/GitSharp/IgnoreRules.cs
        var isIgnored = false;
        var hasMatchingRule = false;

        foreach (var rule in this.rules)
        {
            var isNegativeRule = (rule.PatternFlags & PatternFlags.NEGATION) != 0;

            if (isIgnored == isNegativeRule && rule.IsMatch(path, pathIsDirectory))
            {
                hasMatchingRule = true;
                isIgnored = !isNegativeRule;
            }
        }

        return (hasMatchingRule, isIgnored);
    }
}
