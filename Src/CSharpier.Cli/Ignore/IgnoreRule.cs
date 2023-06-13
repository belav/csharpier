namespace Ignore
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class IgnoreRule
    {
        private static readonly List<Replacer> Replacers = new List<Replacer>
        {
            ReplacerStash.TrailingSpaces,
            ReplacerStash.EscapedSpaces,
            ReplacerStash.LiteralDot,
            ReplacerStash.LiteralPlus,
            // probably not needed
            // ReplacerStash.Metacharacters,
            ReplacerStash.QuestionMark,
            ReplacerStash.NonLeadingSingleStar,
            ReplacerStash.LeadingSingleStar,
            ReplacerStash.LeadingDoubleStar,
            // probably not needed
            // ReplacerStash.MetacharacterSlashAfterLeadingSlash,
            ReplacerStash.MiddleDoubleStar,
            ReplacerStash.LeadingSlash,
            ReplacerStash.TrailingDoubleStar,
            ReplacerStash.OtherDoubleStar,
            ReplacerStash.MiddleSlash,
            ReplacerStash.TrailingSlash,
            ReplacerStash.NoSlash,
            ReplacerStash.NoTrailingSlash,
            ReplacerStash.Ending
        };

        private readonly Regex? parsedRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoreRule"/> class.
        /// Parses the given pattern as per .gitignore spec.
        /// https://git-scm.com/docs/gitignore#_pattern_format.
        /// </summary>
        /// <param name="pattern">Pattern to parse.</param>
        public IgnoreRule(string pattern)
        {
            // A blank line matches no files, so it can serve as a separator for readability.
            if (string.IsNullOrEmpty(pattern.Trim()))
            {
                return;
            }

            // A line starting with # serves as a comment. Put a backslash ("\") in front of the first hash for patterns that begin with a hash.
            if (pattern.StartsWith("#"))
            {
                return;
            }

            // Account for escaped # and !, remove the leading backslash.
            // Also either a pattern will start with \ or with !
            if (pattern.StartsWith("\\!") || pattern.StartsWith("\\#"))
            {
                pattern = pattern.Substring(1);
            }
            else if (pattern.StartsWith("!"))
            {
                Negate = true;
                pattern = pattern.Substring(1);
            }

            foreach (var replacer in Replacers)
            {
                pattern = replacer.Invoke(pattern);
            }

            parsedRegex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public bool Negate { get; }

        public bool IsMatch(string input)
        {
            return parsedRegex != null && parsedRegex.IsMatch(input);
        }
    }
}
