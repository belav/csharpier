using System.Text;
using System.Text.RegularExpressions;

namespace CSharpier.Cli.DotIgnore;

internal static partial class Matcher
{
    private static readonly Regex CharClassRegex = CharClassGeneratedRegex();

    private static readonly Regex InvalidStarStarRegex = InvalidStarStarGeneratedRegex();

    private static readonly Regex EscapedAlphaNumRegex = EscapedAlphaNumGeneratedRegex();

    private static readonly string[] LiteralsToEscapeInRegex =
    [
        ".",
        "$",
        "{",
        "}",
        "(",
        "|",
        ")",
        "+",
    ];

    // Match POSIX char classes with .NET unicode equivalents
    // https://www.regular-expressions.info/posixbrackets.html
    private static readonly Dictionary<string, string> CharClassSubstitutions = new()
    {
        { "[:alnum:]", "a-zA-Z0-9" },
        { "[:alpha:]", "a-zA-Z" },
        { "[:blank:]", @"\p{Zs}\t" },
        { "[:cntrl:]", @"\p{Cc}" },
        { "[:digit:]", @"\d" },
        { "[:graph:]", @"^\p{Z}\p{C}" },
        { "[:lower:]", "a-z" },
        { "[:print:]", @"\p{C}" },
        { "[:punct:]", @"\p{P}" },
        { "[:space:]", @"\s" },
        { "[:upper:]", "A-Z" },
        { "[:xdigit:]", "A-Fa-f0-9" },
    };

    public static bool TryMatch(Regex? regex, string path)
    {
        if (regex == null)
        {
            return false;
        }

        try
        {
            return regex.IsMatch(path);
        }
        catch
        {
            return false;
        }
    }

    // https://git-scm.com/docs/gitignore#_pattern_format
    //
    // FNM_PATHNAME
    //
    // If this flag is set, match a slash in string only with a slash in pattern
    // and not by an asterisk (*) or a question mark (?) metacharacter, nor by a
    // bracket expression ([]) containing a slash.
    //
    // PATTERN FORMAT
    //
    // - A blank line matches no files, so it can serve as a separator for readability.
    //
    // - A line starting with # serves as a comment. Put a backslash ("\") in front of the first
    //   hash for patterns that begin with a hash.
    //
    // - Trailing spaces are ignored unless they are quoted with backslash("\").
    //
    // - An optional prefix "!" which negates the pattern; any matching file excluded by a previous
    //   pattern will become included again. It is not possible to re-include a file if a parent
    //   directory of that file is excluded. Git doesn’t list excluded directories for performance
    //   reasons, so any patterns on contained files have no effect, no matter where they are defined.
    //   Put a backslash("\") in front of the first "!" for patterns that begin with a literal "!",
    //   for example: "\!important!.txt".
    //
    // - If the pattern ends with a slash, it is removed for the purpose of the following description,
    //   but it would only find a match with a directory. In other words, foo/ will match a directory
    //   foo and paths underneath it, but will not match a regular file or a symbolic link foo (this is
    //   consistent with the way how pathspec works in general in Git).
    //
    // - If the pattern does not contain a slash /, Git treats it as a shell glob pattern and checks
    //   for a match against the pathname relative to the location of the.gitignore file (relative to
    //   the toplevel of the work tree if not from a.gitignore file).
    //
    // - Otherwise, Git treats the pattern as a shell glob suitable for consumption by fnmatch(3) with
    //   the FNM_PATHNAME flag: wildcards in the pattern will not match a / in the pathname.
    //   For example, "Documentation/*.html" matches "Documentation/git.html" but not
    //   "Documentation/ppc/ppc.html" or "tools/perf/Documentation/perf.html".
    //
    // - A leading slash matches the beginning of the pathname.
    //   For example, "/*.c" matches "cat-file.c" but not "mozilla-sha1/sha1.c".
    //
    // Two consecutive asterisks("**") in patterns matched against full pathname may have special meaning:
    //
    // - A leading "**" followed by a slash means match in all directories.For example, "**/foo"
    //   matches file or directory "foo" anywhere, the same as pattern "foo". "**/foo/bar" matches
    //   file or directory "bar" anywhere that is directly under directory "foo".
    //
    // - A trailing "/**" matches everything inside.For example, "abc/**" matches all files inside
    //   directory "abc", relative to the location of the.gitignore file, with infinite depth.
    //
    // - A slash followed by two consecutive asterisks then a slash matches zero or more directories.
    //   For example, "a/**/b" matches "a/b", "a/x/b", "a/x/y/b" and so on.
    //
    // - Other consecutive asterisks are considered invalid.

    public static string? ToRegex(string pattern)
    {
        // Double-star is only valid:
        // - at the beginning of a pattern, immediately followed by a slash ('**/c')
        // - at the end of a pattern, immediately preceded by a slash ('a/**')
        // - anywhere in the pattern with a slash immediately before and after ('a/**/c')
        if (InvalidStarStarRegex.IsMatch(pattern))
        {
            return null;
        }

        var patternCharClasses = CharClassRegex.Matches(pattern).Select(m => m.Groups[0].Value);

        if (patternCharClasses.Any(o => !CharClassSubstitutions.ContainsKey(o)))
        {
            // Malformed character class
            return null;
        }

        // Remove single backslashes before alphanumeric chars
        // (escaping these in a glob pattern should have no effect)
        pattern = EscapedAlphaNumRegex.Replace(pattern, "$1");

        var rx = new StringBuilder(pattern);

        foreach (var literal in LiteralsToEscapeInRegex)
        {
            rx.Replace(literal, @"\" + literal);
        }

        foreach (var k in CharClassSubstitutions.Keys)
        {
            rx.Replace(k, CharClassSubstitutions[k]);
        }

        rx.Replace("!", "^");
        rx.Replace("**", "[:STARSTAR:]");
        rx.Replace("*", "[:STAR:]");
        rx.Replace("?", "[:QM:]");

        rx.Insert(0, "^");
        rx.Append('$');

        // TODO: is this only true if PATHMATCH isn't specified?
        // Character class patterns shouldn't match slashes, so we prefix them with
        // negative lookaheads. This is rather harder than it seems, because class
        // patterns can also contain unescaped square brackets...
        var rx2 = pattern.Contains('[')
            ? new StringBuilder(NonPathMatchCharClasses(rx.ToString()))
            : rx;

        // Non-escaped question mark should match any single char except slash
        rx2.Replace(@"\[:QM:]", @"\?");
        rx2.Replace("[:QM:]", "[^/]");

        // Replace star patterns with equivalent regex patterns
        rx2.Replace(@"\[:STAR:]", @"\*");
        rx2.Replace("[:STAR:]", "[^/]*");
        rx2.Replace("[:STARSTAR:]/", "(?:.*?/)?");
        rx2.Replace("[:STARSTAR:]", "(?:.*?)?");

        return rx2.ToString();
    }

    private static string NonPathMatchCharClasses(string p)
    {
        var o = new StringBuilder();
        var inBrackets = false;

        for (var i = 0; i < p.Length; )
        {
            var escaped = i != 0 && p[i - 1] == '\\';

            if (p[i] == '[' && !escaped)
            {
                if (inBrackets)
                {
                    o.Append('\\');
                }
                else
                {
                    if (i < p.Length && p[i + 1] != ':')
                    {
                        o.Append("(?!/)");
                    }

                    inBrackets = true;
                }
            }
            else if (p[i] == ']' && !escaped)
            {
                if (inBrackets && p.IndexOf(']', i + 1) >= p.IndexOf('[', i + 1))
                {
                    inBrackets = false;
                }
            }

            o.Append(p[i++]);
        }

        return o.ToString();
    }

    [GeneratedRegex(@"\[:(?>[a-z]*?):\]", RegexOptions.Compiled)]
    private static partial Regex CharClassGeneratedRegex();

    [GeneratedRegex(@"\*\*[^/\s]|[^/\s]\*\*", RegexOptions.Compiled)]
    private static partial Regex InvalidStarStarGeneratedRegex();

    [GeneratedRegex(@"(?<!\\)\\([a-zA-Z0-9])", RegexOptions.Compiled)]
    private static partial Regex EscapedAlphaNumGeneratedRegex();
}
