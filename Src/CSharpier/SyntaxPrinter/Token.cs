namespace CSharpier.SyntaxPrinter;

using System.Text.RegularExpressions;

internal static class Token
{
    public static Doc PrintWithoutLeadingTrivia(SyntaxToken syntaxToken, FormattingContext context)
    {
        return PrintSyntaxToken(syntaxToken, context, skipLeadingTrivia: true);
    }

    public static Doc PrintWithoutTrailingTrivia(SyntaxToken syntaxToken, FormattingContext context)
    {
        return PrintSyntaxToken(syntaxToken, context, skipTrailingTrivia: true);
    }

    public static Doc Print(SyntaxToken syntaxToken, FormattingContext context)
    {
        return PrintSyntaxToken(syntaxToken, context);
    }

    public static Doc PrintWithSuffix(
        SyntaxToken syntaxToken,
        Doc suffixDoc,
        FormattingContext context,
        bool skipLeadingTrivia = false
    )
    {
        return PrintSyntaxToken(syntaxToken, context, suffixDoc, skipLeadingTrivia);
    }

    internal static readonly string[] lineSeparators = new[] { "\r\n", "\r", "\n" };

    private static Doc PrintSyntaxToken(
        SyntaxToken syntaxToken,
        FormattingContext context,
        Doc? suffixDoc = null,
        bool skipLeadingTrivia = false,
        bool skipTrailingTrivia = false
    )
    {
        if (syntaxToken.RawSyntaxKind() == SyntaxKind.None)
        {
            return Doc.Null;
        }

        var docs = new List<Doc>();
        if (!skipLeadingTrivia && !context.SkipNextLeadingTrivia)
        {
            var leadingTrivia = PrintLeadingTrivia(syntaxToken, context);
            if (leadingTrivia != Doc.Null)
            {
                docs.Add(leadingTrivia);
            }
        }

        context.SkipNextLeadingTrivia = false;

        if (
            (
                syntaxToken.RawSyntaxKind() == SyntaxKind.StringLiteralToken
                && syntaxToken.Text.StartsWith("@")
            )
            || (
                syntaxToken.RawSyntaxKind() == SyntaxKind.InterpolatedStringTextToken
                && syntaxToken.Parent!.Parent
                    is InterpolatedStringExpressionSyntax
                    {
                        StringStartToken:
                        { RawKind: (int)SyntaxKind.InterpolatedVerbatimStringStartToken }
                    }
            )
        )
        {
            var lines = syntaxToken.Text.Replace("\r", string.Empty).Split('\n');
            docs.Add(Doc.Join(Doc.LiteralLine, lines.Select(o => new StringDoc(o))));
        }
        else if (syntaxToken.RawSyntaxKind() is SyntaxKind.MultiLineRawStringLiteralToken)
        {
            var linesIncludingQuotes = syntaxToken.Text.Split(
                lineSeparators,
                StringSplitOptions.None
            );
            var lastLineIsIndented = linesIncludingQuotes[^1][0] is '\t' or ' ';
            var contents = new List<Doc>
            {
                linesIncludingQuotes[0],
                lastLineIsIndented ? Doc.HardLineNoTrim : Doc.LiteralLine,
            };

            var lines = syntaxToken.ValueText.Split(lineSeparators, StringSplitOptions.None);
            foreach (var line in lines)
            {
                contents.Add(line);
                contents.Add(
                    lastLineIsIndented
                        ? string.IsNullOrEmpty(line)
                            ? Doc.HardLine
                            : Doc.HardLineNoTrim
                        : Doc.LiteralLine
                );
            }

            contents.Add(linesIncludingQuotes[^1].TrimStart());

            var hasArgumentParent = syntaxToken.Parent.HasParent(typeof(ArgumentSyntax));

            docs.Add(Doc.IndentIf(!hasArgumentParent, Doc.Concat(contents)));
        }
        else if (
            syntaxToken.RawSyntaxKind()
            is SyntaxKind.InterpolatedMultiLineRawStringStartToken
                or SyntaxKind.InterpolatedRawStringEndToken
        )
        {
            docs.Add(syntaxToken.Text.Trim());
        }
        else
        {
            docs.Add(syntaxToken.Text);
        }

        if (!skipTrailingTrivia)
        {
            var trailingTrivia = PrintTrailingTrivia(syntaxToken);
            if (trailingTrivia != Doc.Null)
            {
                docs.Add(trailingTrivia);
            }
        }

        if (suffixDoc != null)
        {
            docs.Add(suffixDoc);
        }

        return docs.Count switch
        {
            <= 0 => Doc.Null,
            1 => docs.First(),
            _ => Doc.Concat(docs),
        };
    }

    public static Doc PrintLeadingTrivia(SyntaxToken syntaxToken, FormattingContext context)
    {
        var isClosingBrace =
            syntaxToken.RawSyntaxKind() == SyntaxKind.CloseBraceToken
            || syntaxToken.Parent is CollectionExpressionSyntax
                && syntaxToken.RawSyntaxKind() == SyntaxKind.CloseBracketToken;

        var printedTrivia = PrivatePrintLeadingTrivia(
            syntaxToken.LeadingTrivia,
            context,
            skipLastHardline: isClosingBrace
        );

        var hasDirective = syntaxToken.LeadingTrivia.Any(o => o.IsDirective);

        if (hasDirective)
        {
            // the leading trivia "always fits" for purposes of deciding when to break Lines, so the method call after a false #if directive doesn't break when it actually fits
            /*
            #if CONDITION
                        if (true)
                        {
                            return;
                        }
            #endif
            SomeObject.CallMethod().CallOtherMethod(shouldNotBreak);
            */
            printedTrivia = Doc.AlwaysFits(printedTrivia);
        }

        if (syntaxToken.RawSyntaxKind() != SyntaxKind.CloseBraceToken)
        {
            return printedTrivia;
        }

        Doc extraNewLines = Doc.Null;

        if (hasDirective || syntaxToken.LeadingTrivia.Any(o => o.IsComment()))
        {
            extraNewLines = ExtraNewLines.Print(syntaxToken.LeadingTrivia);
        }

        return printedTrivia != Doc.Null || extraNewLines != Doc.Null
            ? Doc.Concat(
                extraNewLines,
                Doc.IndentIf(printedTrivia != Doc.Null, printedTrivia),
                Doc.HardLine
            )
            : printedTrivia;
    }

    public static Doc PrintLeadingTrivia(SyntaxTriviaList leadingTrivia, FormattingContext context)
    {
        return PrivatePrintLeadingTrivia(leadingTrivia, context);
    }

    public static Doc PrintLeadingTriviaWithNewLines(
        SyntaxTriviaList leadingTrivia,
        FormattingContext context
    )
    {
        return PrivatePrintLeadingTrivia(leadingTrivia, context, includeInitialNewLines: true);
    }

    private static Doc PrivatePrintLeadingTrivia(
        SyntaxTriviaList leadingTrivia,
        FormattingContext context,
        bool includeInitialNewLines = false,
        bool skipLastHardline = false
    )
    {
        var docs = new List<Doc>();

        // we don't print any new lines until we run into a comment or directive
        // the PrintExtraNewLines method takes care of printing the initial new lines for a given node
        var printNewLines = includeInitialNewLines;

        for (var x = 0; x < leadingTrivia.Count; x++)
        {
            var trivia = leadingTrivia[x];
            var kind = trivia.RawSyntaxKind();

            if (printNewLines && kind == SyntaxKind.EndOfLineTrivia)
            {
                docs.Add(Doc.HardLineSkipBreakIfFirstInGroup);
            }
            if (kind is not (SyntaxKind.EndOfLineTrivia or SyntaxKind.WhitespaceTrivia))
            {
                printNewLines = true;
            }

            void AddLeadingComment(CommentType commentType)
            {
                var comment = trivia.ToFullString().TrimEnd('\n', '\r');
                if (
                    commentType == CommentType.MultiLine
                    && x > 0
                    && leadingTrivia[x - 1].RawSyntaxKind() is SyntaxKind.WhitespaceTrivia
                )
                {
                    comment = leadingTrivia[x - 1] + comment;
                }

                docs.Add(Doc.LeadingComment(comment, commentType));
            }

            if (IsSingleLineComment(kind))
            {
                AddLeadingComment(CommentType.SingleLine);
                docs.Add(
                    kind == SyntaxKind.SingleLineDocumentationCommentTrivia
                        ? Doc.HardLineSkipBreakIfFirstInGroup
                        : Doc.Null
                );
            }
            else if (IsMultiLineComment(kind))
            {
                AddLeadingComment(CommentType.MultiLine);
            }
            else if (kind == SyntaxKind.DisabledTextTrivia)
            {
                docs.Add(Doc.Trim, trivia.ToString());
            }
            else if (IsRegion(kind))
            {
                var triviaText = trivia.ToString();
                docs.Add(Doc.HardLineIfNoPreviousLine);
                docs.Add(Doc.Trim);
                docs.Add(
                    kind == SyntaxKind.RegionDirectiveTrivia
                        ? Doc.BeginRegion(triviaText)
                        : Doc.EndRegion(triviaText)
                );
                docs.Add(Doc.HardLine);
            }
            else if (trivia.IsDirective)
            {
                var triviaText = trivia.ToString();

                docs.Add(
                    // adding two of these to ensure we get a new line when a directive follows a trailing comment
                    Doc.HardLineIfNoPreviousLineSkipBreakIfFirstInGroup,
                    Doc.HardLineIfNoPreviousLineSkipBreakIfFirstInGroup,
                    Doc.Trim,
                    Doc.Directive(triviaText),
                    Doc.HardLineSkipBreakIfFirstInGroup
                );

                // keep one line after an #endif if there is at least one
                if (kind is SyntaxKind.EndIfDirectiveTrivia)
                {
                    if (
                        x + 1 < leadingTrivia.Count
                        && leadingTrivia[x + 1].RawSyntaxKind() is SyntaxKind.EndOfLineTrivia
                    )
                    {
                        x++;
                        docs.Add(Doc.HardLineSkipBreakIfFirstInGroup);
                    }
                    printNewLines = false;
                }
            }
        }

        while (skipLastHardline && docs.Any() && docs.Last() is HardLine or NullDoc)
        {
            docs.RemoveAt(docs.Count - 1);
        }

        if (context.NextTriviaNeedsLine)
        {
            if (leadingTrivia.Any(o => o.RawSyntaxKind() is SyntaxKind.IfDirectiveTrivia))
            {
                docs.Insert(0, Doc.HardLineSkipBreakIfFirstInGroup);
            }
            else
            {
                var index = docs.Count - 1;
                while (
                    index >= 0
                    && (docs[index] is HardLine or LeadingComment || docs[index] == Doc.Null)
                )
                {
                    index--;
                }
                // this handles an edge case where we get here but already added the line
                // it relates to the fact that single line comments include new line directives
                if (
                    index + 2 >= docs.Count
                    || !(docs[index + 1] is HardLine && docs[index + 2] is HardLine)
                )
                {
                    docs.Insert(index + 1, Doc.HardLineSkipBreakIfFirstInGroup);
                }
            }
            context.NextTriviaNeedsLine = false;
        }

        return docs.Count > 0 ? Doc.Concat(docs) : Doc.Null;
    }

    private static bool IsSingleLineComment(SyntaxKind kind) =>
        kind
            is SyntaxKind.SingleLineDocumentationCommentTrivia
                or SyntaxKind.SingleLineCommentTrivia;

    private static bool IsMultiLineComment(SyntaxKind kind) =>
        kind is SyntaxKind.MultiLineCommentTrivia or SyntaxKind.MultiLineDocumentationCommentTrivia;

    private static bool IsRegion(SyntaxKind kind) =>
        kind is SyntaxKind.RegionDirectiveTrivia or SyntaxKind.EndRegionDirectiveTrivia;

    public static Doc PrintTrailingTrivia(SyntaxToken node)
    {
        return PrintTrailingTrivia(node.TrailingTrivia);
    }

    private static Doc PrintTrailingTrivia(SyntaxTriviaList trailingTrivia)
    {
        var docs = new List<Doc>();
        foreach (var trivia in trailingTrivia)
        {
            if (trivia.RawSyntaxKind() == SyntaxKind.SingleLineCommentTrivia)
            {
                docs.Add(Doc.TrailingComment(trivia.ToString(), CommentType.SingleLine));
            }
            else if (trivia.RawSyntaxKind() == SyntaxKind.MultiLineCommentTrivia)
            {
                docs.Add(" ", Doc.TrailingComment(trivia.ToString(), CommentType.MultiLine));
            }
        }

        return docs.Count > 0 ? Doc.Concat(docs) : Doc.Null;
    }

    public static bool HasComments(SyntaxToken syntaxToken)
    {
        return syntaxToken.LeadingTrivia.Any(o =>
                o.RawSyntaxKind() is not (SyntaxKind.WhitespaceTrivia or SyntaxKind.EndOfLineTrivia)
            )
            || syntaxToken.TrailingTrivia.Any(o =>
                o.RawSyntaxKind() is not (SyntaxKind.WhitespaceTrivia or SyntaxKind.EndOfLineTrivia)
            );
    }

    public static bool HasLeadingCommentMatching(SyntaxNode node, Regex regex)
    {
        return node.GetLeadingTrivia()
            .Any(o =>
                o.RawSyntaxKind() is SyntaxKind.SingleLineCommentTrivia
                && regex.IsMatch(o.ToString())
            );
    }

    public static bool HasLeadingCommentMatching(SyntaxToken token, Regex regex)
    {
        return token.LeadingTrivia.Any(o =>
            o.RawSyntaxKind() is SyntaxKind.SingleLineCommentTrivia && regex.IsMatch(o.ToString())
        );
    }
}
