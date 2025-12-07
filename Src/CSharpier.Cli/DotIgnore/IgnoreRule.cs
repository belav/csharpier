using System.Text.RegularExpressions;

namespace CSharpier.Cli.DotIgnore;

internal class IgnoreRule
{
    private static readonly char[] _wildcardChars = ['*', '[', '?'];

    private readonly int wildcardIndex;
    private readonly Regex? regex;
    private readonly StringComparison stringComparison = StringComparison.Ordinal;

    public string Pattern { get; }
    public PatternFlags PatternFlags { get; }

    public IgnoreRule(string pattern)
    {
        this.Pattern = pattern;

        this.PatternFlags = PatternFlags.NONE;

        // If the pattern starts with an exclamation mark, it's a negation pattern
        // Once we know that, we can remove the exclamation mark (so the pattern behaves
        // just like any other), then just negate the match result when we return it
        if (this.Pattern.StartsWith('!'))
        {
            this.PatternFlags |= PatternFlags.NEGATION;
            this.Pattern = this.Pattern[1..];
        }

        // If the pattern starts with a forward slash, it should only match an absolute path
        if (this.Pattern.StartsWith('/'))
        {
            this.PatternFlags |= PatternFlags.ABSOLUTE_PATH;
            this.Pattern = this.Pattern[1..];
        }

        // If the pattern ends with a forward slash, it should only match a directory
        // Again though, once we know that we can remove the slash to normalise the pattern
        if (this.Pattern.EndsWith('/'))
        {
            this.PatternFlags |= PatternFlags.DIRECTORY;
            this.Pattern = this.Pattern[..^1];
        }

        this.wildcardIndex = this.Pattern.IndexOfAny(_wildcardChars);

        var rxPattern = Matcher.ToRegex(this.Pattern);

        // If rxPattern is null, an invalid pattern was passed to ToRegex, so it cannot match
        if (!string.IsNullOrEmpty(rxPattern))
        {
            var rxOptions = RegexOptions.Compiled;

            this.regex = new Regex(rxPattern, rxOptions);
        }
    }

    public bool IsMatch(string path, bool pathIsDirectory)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(path));
        }

        // .gitignore files use Unix paths (with a forward slash separator),
        // so make sure our input also uses forward slashes
        path = path.NormalisePath().TrimStart('/');

        // Shortcut return if the pattern is directory-only and the path isn't a directory
        // This has to be determined by the OS (at least that's the only reliable way),
        // so we pass that information in as a boolean so the consuming code can provide it
        if ((this.PatternFlags & PatternFlags.DIRECTORY) != 0 && !pathIsDirectory)
        {
            return false;
        }

        // If the pattern is an absolute path pattern, the path must start with the part of the pattern
        // before any wildcards occur. If it doesn't, we can just return a negative match
        var patternBeforeFirstWildcard =
            this.wildcardIndex != -1 ? this.Pattern[..this.wildcardIndex] : this.Pattern;

        if (
            (this.PatternFlags & PatternFlags.ABSOLUTE_PATH) != 0
            && !path.StartsWith(patternBeforeFirstWildcard, this.stringComparison)
        )
        {
            return false;
        }

        // If we got this far, we can't figure out the match with simple
        // string matching, so use our regex match function

        // If the *pattern* does not contain any slashes, it should match *any*
        // occurence, *anywhere* within the path (e.g. '*.jpg' should match
        // 'a.jpg', 'a/b.jpg', 'a/b/c.jpg'), so try matching before each slash
        if (!this.Pattern.Contains('/') && path.Contains('/'))
        {
            return path.Split('/').Any(segment => Matcher.TryMatch(this.regex, segment));
        }

        // If the *path* doesn't contain any slashes, we should skip over the conditional above
        return Matcher.TryMatch(this.regex, path);
    }

    public override string ToString()
    {
        return this.Pattern;
    }
}
