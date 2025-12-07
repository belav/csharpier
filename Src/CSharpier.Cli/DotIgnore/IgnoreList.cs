using System.IO.Abstractions;

namespace CSharpier.Cli.DotIgnore;

internal class IgnoreList
{
    private IgnoreList() { }

    private static readonly string[] alwaysIgnoredText =
    [
        "**/bin",
        "**/node_modules",
        "**/obj",
        "**/.git",
    ];
    private readonly List<IgnoreRule> _rules = [];

    public static async Task<IgnoreList> CreateAsync(
        IFileSystem fileSystem,
        string? ignoreFilePath,
        CancellationToken cancellationToken
    )
    {
        var ignoreList = new IgnoreList();
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
        var ruleList = GetValidRuleLines(rules).Select(o => new IgnoreRule(o, flags));

        this._rules.AddRange(ruleList);
    }

    public (bool hasMatchingRule, bool isIgnored) IsIgnored(string filePath)
    {
        return this.IsIgnored(filePath, false);
    }

    private (bool hasMatchingRule, bool isIgnored) IsIgnored(string path, bool pathIsDirectory)
    {
        // TODO I think we only need to check this if we have a matching rule and it returns !isIgnored
        var ancestorIgnored = this.IsAnyParentDirectoryIgnored(path);

        if (ancestorIgnored)
        {
            return (true, true);
        }

        return this.IsPathIgnored(path, pathIsDirectory);
    }

    // Exclude all comment or whitespace lines
    // Note that we store the line numbers (if flag set) *before* filtering out
    // comments and whitespace, otherwise they don't match up with the source file
    private static IEnumerable<string> GetValidRuleLines(IEnumerable<string> rules)
    {
        return rules.Select(o => o.Trim()).Where(line => line.Length > 0 && !line.StartsWith('#'));
    }

    // TODO optimize this?
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

        foreach (var rule in this._rules)
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
