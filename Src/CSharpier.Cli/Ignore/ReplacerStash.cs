namespace Ignore
{
    using System;
    using System.Text.RegularExpressions;

    public static class ReplacerStash
    {
        public static readonly Replacer TrailingSpaces = new Replacer(
            name: nameof(TrailingSpaces),
            regex: new Regex(@"\\?\s+$", RegexOptions.Compiled),
            replacer: match =>
                match.Value.IndexOf("\\", StringComparison.Ordinal) == 0 ? " " : string.Empty
        );

        public static readonly Replacer EscapedSpaces = new Replacer(
            name: nameof(EscapedSpaces),
            regex: new Regex(@"\\\s", RegexOptions.Compiled),
            replacer: match => " "
        );

        public static readonly Replacer LiteralPlus = new Replacer(
            name: nameof(LiteralPlus),
            regex: new Regex(@"\+", RegexOptions.Compiled),
            replacer: match => @"\+"
        );

        // a ? matches any character other than a /
        public static readonly Replacer QuestionMark = new Replacer(
            name: nameof(QuestionMark),
            regex: new Regex(@"(?!\\)\?", RegexOptions.Compiled),
            replacer: match => "[^/]"
        );

        // a leading / matches the beginning of the path
        // eg. /fake.c only matches fake.c, not src/fake.c
        public static readonly Replacer LeadingSlash = new Replacer(
            name: nameof(LeadingSlash),
            regex: new Regex(@"^\/", RegexOptions.Compiled),
            replacer: match => "^"
        );

        public static readonly Replacer MetacharacterSlashAfterLeadingSlash = new Replacer(
            name: nameof(MetacharacterSlashAfterLeadingSlash),
            regex: new Regex(@"\/", RegexOptions.Compiled),
            replacer: match => "\\/"
        );

        /// <summary>
        /// From gitignore:
        /// A leading "**" followed by a slash means match in all directories.
        /// For example, "**/foo" matches file or directory "foo" anywhere, the same as pattern "foo".
        /// "**/foo/bar" matches file or directory "bar" anywhere that is directly under directory "foo".
        /// </summary>
        public static readonly Replacer LeadingDoubleStar = new Replacer(
            name: nameof(LeadingDoubleStar),
            regex: new Regex(@"^(\*\*/|\*\*)", RegexOptions.Compiled),
            replacer: match => @".*"
        );

        /// <summary>
        /// From gitignore:
        /// A slash followed by two consecutive asterisks then a slash matches zero or more directories.
        /// For example, "a/**/b" matches "a/b", "a/x/b", "a/x/y/b" and so on.
        /// </summary>
        public static readonly Replacer MiddleDoubleStar = new Replacer(
            name: nameof(MiddleDoubleStar),
            regex: new Regex(@"(?<=/)\*\*/", RegexOptions.Compiled),
            replacer: match => @"(.*/)?"
        );

        /// <summary>
        /// From gitignore:
        /// A trailing "/**" matches everything inside. For example,
        /// "abc/**" matches all files inside directory "abc",
        /// relative to the location of the .gitignore file, with infinite depth.
        /// </summary>
        public static readonly Replacer TrailingDoubleStar = new Replacer(
            name: nameof(TrailingDoubleStar),
            regex: new Regex(@"\*\*$", RegexOptions.Compiled),
            replacer: match => @".*$"
        );

        /// <summary>
        /// Undocumented cases like foo/**.ps
        /// Treat ** as match any character other than /.
        /// </summary>
        public static readonly Replacer OtherDoubleStar = new Replacer(
            name: nameof(TrailingDoubleStar),
            regex: new Regex(@"\*\*", RegexOptions.Compiled),
            replacer: match => @"[^/]*"
        );

        /// <summary>
        /// If there is a separator at the beginning or middle (or both) of the pattern,
        /// then the pattern is relative to the directory level of the particular .gitignore file itself.
        /// Otherwise the pattern may also match at any level below the .gitignore level.
        /// The leading slash should be gone after using <see cref="LeadingSlash"/>.
        /// So put a ^ in the beginning of the match.
        /// </summary>
        public static readonly Replacer MiddleSlash = new Replacer(
            name: nameof(MiddleSlash),
            regex: new Regex(@"^([^/\^]+/[^/]+)"),
            replacer: match => $"^{match.Groups[1]}"
        );

        /// <summary>
        /// From gitignore:
        /// If there is a separator at the end of the pattern then the pattern will only match directories,
        /// otherwise the pattern can match both files and directories.
        ///
        /// This regex handles the paths with trailing slash present.
        /// </summary>
        public static readonly Replacer TrailingSlash = new Replacer(
            name: nameof(TrailingSlash),
            regex: new Regex(@"^([^/]+)/$"),
            replacer: match => $@"(/|^){match.Groups[1]}/"
        );

        /// <summary>
        /// From gitignore:
        /// If there is a separator at the end of the pattern then the pattern will only match directories,
        /// otherwise the pattern can match both files and directories.
        ///
        /// This pattern handles the paths with no trailing slash.
        /// </summary>
        public static readonly Replacer NoTrailingSlash = new Replacer(
            name: nameof(NoTrailingSlash),
            regex: new Regex(@"([^/$]+)$"),
            replacer: match => $@"{match.Groups[1]}(/.*)?$"
        );

        /// <summary>
        /// From gitignore:
        /// An asterisk "*" matches anything except a slash.
        ///
        /// Replaces single * with anything other than a /.
        /// Unless the star is in the beginning of the pattern.
        /// </summary>
        public static readonly Replacer NonLeadingSingleStar = new Replacer(
            name: nameof(NonLeadingSingleStar),
            regex: new Regex(@"(?<!^)(?<!\*)\*(?!\*)"),
            replacer: match => @"[^/]*"
        );

        public static readonly Replacer LeadingSingleStar = new Replacer(
            name: nameof(LeadingSingleStar),
            regex: new Regex(@"^(?<!\*)\*(?!\*)"),
            replacer: match => @".*"
        );

        public static readonly Replacer Ending = new Replacer(
            name: nameof(Ending),
            regex: new Regex(@"([^/$]+)$"),
            replacer: match => $"{match.Groups[1]}$"
        );

        public static readonly Replacer LiteralDot = new Replacer(
            name: nameof(LiteralDot),
            regex: new Regex(@"\."),
            replacer: match => @"\."
        );

        public static readonly Replacer NoSlash = new Replacer(
            name: nameof(NoSlash),
            regex: new Regex(@"(^[^/]*$)"),
            replacer: match => $"(^|/){match.Groups[1]}"
        );
    }
}
