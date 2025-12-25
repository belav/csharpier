using System.Text;
using System.Text.RegularExpressions;

namespace CSharpier.Cli.DotIgnore;

internal static partial class Matcher
{
    private static readonly Regex CharClassRegex = CharClassGeneratedRegex();

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

    public static string? ToRegex(string pattern)
    {
        // handles the case of foo/**.ps which is equivalent to foo/*.ps
        pattern = pattern.Replace("**.", "*.");

        var patternCharClasses = CharClassRegex.Matches(pattern).Select(m => m.Groups[0].Value);

        if (patternCharClasses.Any(o => !CharClassSubstitutions.ContainsKey(o)))
        {
            // Malformed character class
            return null;
        }

        // Remove single backslashes before alphanumeric chars
        // (escaping these in a glob pattern should have no effect)
        pattern = EscapedAlphaNumRegex.Replace(pattern, "$1");

        var regex = new StringBuilder(pattern);

        foreach (var literal in LiteralsToEscapeInRegex)
        {
            regex.Replace(literal, @"\" + literal);
        }

        foreach (var k in CharClassSubstitutions.Keys)
        {
            regex.Replace(k, CharClassSubstitutions[k]);
        }

        regex.Replace("\\!", "!");
        regex.Replace("**", "[:STARSTAR:]");
        regex.Replace("*", "[:STAR:]");
        regex.Replace("?", "[:QM:]");

        regex.Insert(0, "^");
        regex.Append('$');

        // TODO: is this only true if PATHMATCH isn't specified?
        // Character class patterns shouldn't match slashes, so we prefix them with
        // negative lookaheads. This is rather harder than it seems, because class
        // patterns can also contain unescaped square brackets...
        regex = pattern.Contains('[')
            ? new StringBuilder(NonPathMatchCharClasses(regex.ToString()))
            : regex;

        // Non-escaped question mark should match any single char except slash
        regex.Replace(@"\[:QM:]", @"\?");
        regex.Replace("[:QM:]", "[^/]");

        // Replace star patterns with equivalent regex patterns
        regex.Replace(@"\[:STAR:]", @"\*");
        regex.Replace("[:STAR:]", "[^/]*");
        regex.Replace("[:STARSTAR:]/", "(?:.*?/)?");
        regex.Replace("[:STARSTAR:]", "(?:.*?)?");

        // TODO #1771 why do we not need to do this in the regex? maybe some spaces in things files/directories won't work as expected
        // regex.Replace("\\ ", " ");

        return regex.ToString();
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

    [GeneratedRegex(@"(?<!\\)\\([a-zA-Z0-9])", RegexOptions.Compiled)]
    private static partial Regex EscapedAlphaNumGeneratedRegex();
}
