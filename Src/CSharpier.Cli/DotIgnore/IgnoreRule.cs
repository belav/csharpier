using System.Text.RegularExpressions;

namespace CSharpier.Cli.DotIgnore;

internal class IgnoreRule
{
    private static readonly char[] _wildcardChars = ['*', '[', '?'];

    private readonly int wildcardIndex;
    private readonly string patternBeforeFirstWildcard;
    private readonly bool patternContainsSlash;
    private readonly Regex? regex;
    private readonly StringComparison stringComparison = StringComparison.Ordinal;
    private readonly MatchKind matchKind;
    private readonly string literalPrefix = string.Empty;
    private readonly string literalSuffix = string.Empty;

    private enum MatchKind
    {
        Regex,
        Exact,
        Prefix,
        Suffix,
        PrefixAndSuffix,
    }

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
        // Again though, once we know that we can remove the slash to normalize the pattern
        if (this.Pattern.EndsWith('/'))
        {
            this.PatternFlags |= PatternFlags.DIRECTORY;
            this.Pattern = this.Pattern[..^1];
        }

        this.wildcardIndex = this.Pattern.IndexOfAny(_wildcardChars);

        var rxPattern = Matcher.ToRegex(this.Pattern);

        this.Pattern = this.Pattern.Replace("\\ ", " ");

        this.patternBeforeFirstWildcard =
            this.wildcardIndex != -1 ? this.Pattern[..this.wildcardIndex] : this.Pattern;
        this.patternContainsSlash = this.Pattern.Contains('/');

        this.matchKind = this.DetermineMatchKind(out this.literalPrefix, out this.literalSuffix);
        if (this.matchKind is not MatchKind.Regex)
        {
            return;
        }

        // If rxPattern is null, an invalid pattern was passed to ToRegex, so it cannot match
        if (!string.IsNullOrEmpty(rxPattern))
        {
            try
            {
                // RegexOptions.Compiled costs more than it saves unless
                // ~10,000 files are checked. It is also much slower to create
                // which can negatively affect csharpier server
                this.regex = new Regex(rxPattern, RegexOptions.None);
            }
            catch (RegexParseException ex)
                when (ex.Message
                    == "Invalid pattern '^Icon(?!/)[$' at offset 12. Unterminated [] set."
                )
            {
                // old macOS uses 'Icon\r' in folder names, which needs this in a .gitignore Icon[\r]\n
                // instead of dealing with that, just don't treat this line as regex
            }
        }
    }

    private MatchKind DetermineMatchKind(out string prefix, out string suffix)
    {
        prefix = string.Empty;
        suffix = string.Empty;

        if (this.Pattern.AsSpan().IndexOfAny('\\', '?', '[') >= 0 || this.Pattern.Contains(']'))
        {
            return MatchKind.Regex;
        }

        var star = this.Pattern.IndexOf('*', StringComparison.Ordinal);
        if (star >= 0 && this.Pattern.IndexOf('*', star + 1) >= 0)
        {
            return MatchKind.Regex;
        }

        if (star < 0)
        {
            prefix = this.Pattern;
            return MatchKind.Exact;
        }

        if (this.patternContainsSlash)
        {
            return MatchKind.Regex;
        }

        prefix = this.Pattern[..star];
        suffix = this.Pattern[(star + 1)..];

        if (prefix.Length == 0)
        {
            return MatchKind.Suffix;
        }

        return suffix.Length == 0 ? MatchKind.Prefix : MatchKind.PrefixAndSuffix;
    }

    private bool Matches(ReadOnlySpan<char> value)
    {
        return this.matchKind switch
        {
            MatchKind.Exact => value.Equals(this.literalPrefix, StringComparison.Ordinal),
            MatchKind.Prefix => value.StartsWith(this.literalPrefix, StringComparison.Ordinal)
                && StarRegionHasNoSlash(value, this.literalPrefix.Length, 0),
            MatchKind.Suffix => value.EndsWith(this.literalSuffix, StringComparison.Ordinal)
                && StarRegionHasNoSlash(value, 0, this.literalSuffix.Length),
            MatchKind.PrefixAndSuffix => value.Length
                >= this.literalPrefix.Length + this.literalSuffix.Length
                && value.StartsWith(this.literalPrefix, StringComparison.Ordinal)
                && value.EndsWith(this.literalSuffix, StringComparison.Ordinal)
                && StarRegionHasNoSlash(
                    value,
                    this.literalPrefix.Length,
                    this.literalSuffix.Length
                ),
            _ => Matcher.TryMatch(this.regex, value),
        };
    }

    // a star in the pattern becomes [^/]*, so it cannot stand in for a slash even when the
    // whole path is being matched rather than a single segment
    private static bool StarRegionHasNoSlash(
        ReadOnlySpan<char> value,
        int prefixLength,
        int suffixLength
    )
    {
        return !value[prefixLength..(value.Length - suffixLength)].Contains('/');
    }

    // path must already be normalised; IgnoreList.IsIgnored is where that happens
    public bool IsMatch(string path, bool pathIsDirectory)
    {
        // Shortcut return if the pattern is directory-only and the path isn't a directory
        // This has to be determined by the OS (at least that's the only reliable way),
        // so we pass that information in as a boolean so the consuming code can provide it
        if ((this.PatternFlags & PatternFlags.DIRECTORY) != 0 && !pathIsDirectory)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or empty", nameof(path));
        }

        // If the pattern is an absolute path pattern, the path must start with the part of the pattern
        // before any wildcards occur. If it doesn't, we can just return a negative match
        if (
            (this.PatternFlags & PatternFlags.ABSOLUTE_PATH) != 0
            && !path.StartsWith(this.patternBeforeFirstWildcard, this.stringComparison)
        )
        {
            return false;
        }

        // If we got this far, we can't figure out the match with simple
        // string matching, so use our regex match function

        // If the *pattern* does not contain any slashes, it should match *any*
        // occurence, *anywhere* within the path (e.g. '*.jpg' should match
        // 'a.jpg', 'a/b.jpg', 'a/b/c.jpg'), so try matching before each slash
        if (
            (this.PatternFlags & PatternFlags.ABSOLUTE_PATH) == 0
            && !this.patternContainsSlash
            && path.Contains('/')
        )
        {
            var remaining = path.AsSpan();
            while (true)
            {
                var separator = remaining.IndexOf('/');
                var segment = separator < 0 ? remaining : remaining[..separator];
                if (this.Matches(segment))
                {
                    return true;
                }

                if (separator < 0)
                {
                    return false;
                }

                remaining = remaining[(separator + 1)..];
            }
        }

        // If the *path* doesn't contain any slashes, we should skip over the conditional above
        return this.Matches(path);
    }

    public override string ToString()
    {
        return this.Pattern + " " + this.PatternFlags;
    }
}
