namespace CSharpier.SyntaxPrinter;

internal static class Token
{
    [ThreadStatic]
    public static bool ShouldSkipNextLeadingTrivia;

    public static Doc PrintWithoutLeadingTrivia(SyntaxToken syntaxToken)
    {
        return PrintSyntaxToken(syntaxToken, null, skipLeadingTrivia: true);
    }

    public static Doc Print(SyntaxToken syntaxToken)
    {
        return PrintSyntaxToken(syntaxToken);
    }

    public static Doc PrintWithSuffix(SyntaxToken syntaxToken, Doc suffixDoc)
    {
        return PrintSyntaxToken(syntaxToken, suffixDoc);
    }

    private static Doc PrintSyntaxToken(
        SyntaxToken syntaxToken,
        Doc? suffixDoc = null,
        bool skipLeadingTrivia = false
    )
    {
        if (syntaxToken.Kind() == SyntaxKind.None)
        {
            return Doc.Null;
        }

        var docs = new List<Doc>();
        if (!skipLeadingTrivia && !ShouldSkipNextLeadingTrivia)
        {
            var leadingTrivia = PrintLeadingTrivia(syntaxToken);
            if (leadingTrivia != Doc.Null)
            {
                docs.Add(leadingTrivia);
            }
        }

        ShouldSkipNextLeadingTrivia = false;

        if (
            (
                syntaxToken.Kind() == SyntaxKind.StringLiteralToken
                && syntaxToken.Text.StartsWith("@")
            )
            || (
                syntaxToken.Kind() == SyntaxKind.InterpolatedStringTextToken
                && syntaxToken.Parent!.Parent
                    is InterpolatedStringExpressionSyntax
                    {
                        StringStartToken:
                        { RawKind: (int)SyntaxKind.InterpolatedVerbatimStringStartToken }
                    }
            )
        )
        {
            var lines = syntaxToken.Text.Replace("\r", string.Empty).Split(new[] { '\n' });
            docs.Add(Doc.Join(Doc.LiteralLine, lines.Select(o => new StringDoc(o))));
        }
        else
        {
            docs.Add(syntaxToken.Text);
        }
        var trailingTrivia = PrintTrailingTrivia(syntaxToken);
        if (trailingTrivia != Doc.Null)
        {
            docs.Add(trailingTrivia);
        }

        if (suffixDoc != null)
        {
            docs.Add(suffixDoc);
        }

        return docs.Count switch
        {
            <= 0 => Doc.Null,
            1 => docs.First(),
            _ => Doc.Concat(docs)
        };
    }

    public static Doc PrintLeadingTrivia(SyntaxToken syntaxToken)
    {
        var isClosingBrace = syntaxToken.Kind() == SyntaxKind.CloseBraceToken;

        var printedTrivia = PrivatePrintLeadingTrivia(
            syntaxToken.LeadingTrivia,
            includeInitialNewLines: isClosingBrace,
            skipLastHardline: isClosingBrace,
            useLastWasLine: isClosingBrace
        );

        return isClosingBrace && printedTrivia != Doc.Null
          ? Doc.Concat(Doc.Indent(printedTrivia), Doc.HardLine)
          : printedTrivia;
    }

    public static Doc PrintLeadingTrivia(SyntaxTriviaList leadingTrivia)
    {
        return PrivatePrintLeadingTrivia(leadingTrivia);
    }

    public static Doc PrintLeadingTriviaWithNewLines(SyntaxTriviaList leadingTrivia)
    {
        return PrivatePrintLeadingTrivia(leadingTrivia, includeInitialNewLines: true);
    }

    private static Doc PrivatePrintLeadingTrivia(
        SyntaxTriviaList leadingTrivia,
        bool includeInitialNewLines = false,
        bool skipLastHardline = false,
        bool useLastWasLine = false
    )
    {
        var docs = new List<Doc>();

        // we don't print any new lines until we run into a comment or directive
        // the PrintExtraNewLines method takes care of printing the initial new lines for a given node
        var printNewLines = includeInitialNewLines;
        var lastWasLine = false;
        for (var x = 0; x < leadingTrivia.Count; x++)
        {
            var trivia = leadingTrivia[x];
            var kind = trivia.Kind();

            if (
                printNewLines
                && (!lastWasLine || !useLastWasLine)
                && kind == SyntaxKind.EndOfLineTrivia
            )
            {
                lastWasLine = true;
                docs.Add(Doc.HardLineSkipBreakIfFirstInGroup);
            }
            if (kind != SyntaxKind.EndOfLineTrivia && kind != SyntaxKind.WhitespaceTrivia)
            {
                printNewLines = true;
                lastWasLine = false;
            }
            if (IsSingleLineComment(kind))
            {
                docs.Add(
                    Doc.LeadingComment(
                        trivia.ToFullString().TrimEnd('\n', '\r'),
                        CommentType.SingleLine
                    ),
                    kind == SyntaxKind.SingleLineDocumentationCommentTrivia
                      ? Doc.HardLineSkipBreakIfFirstInGroup
                      : Doc.Null
                );
            }
            else if (IsMultiLineComment(kind))
            {
                docs.Add(
                    Doc.LeadingComment(
                        trivia.ToFullString().TrimEnd('\n', '\r'),
                        CommentType.MultiLine
                    )
                );
            }
            else if (kind == SyntaxKind.DisabledTextTrivia)
            {
                docs.Add(Doc.Trim, trivia.ToString());
            }
            else if (IsDirective(kind) || IsRegion(kind))
            {
                var triviaText = trivia.ToString();
                if (
                    IsRegion(kind)
                    && x > 0
                    && leadingTrivia[x - 1].Kind() == SyntaxKind.WhitespaceTrivia
                )
                {
                    triviaText = leadingTrivia[x - 1] + triviaText;
                }

                docs.Add(
                    // adding two of these to ensure we get a new line when a directive follows a trailing comment
                    Doc.HardLineIfNoPreviousLineSkipBreakIfFirstInGroup,
                    Doc.HardLineIfNoPreviousLineSkipBreakIfFirstInGroup,
                    Doc.Trim,
                    Doc.Directive(triviaText),
                    Doc.HardLineSkipBreakIfFirstInGroup
                );
            }

            PreprocessorSymbols.AddSymbolSet(trivia);
        }

        if (skipLastHardline && docs.Any() && docs.Last() is HardLine)
        {
            docs.RemoveAt(docs.Count - 1);
        }

        return docs.Count > 0 ? Doc.Concat(docs) : Doc.Null;
    }

    private static bool IsSingleLineComment(SyntaxKind kind) =>
        kind
            is SyntaxKind.SingleLineDocumentationCommentTrivia
                or SyntaxKind.SingleLineCommentTrivia;

    private static bool IsMultiLineComment(SyntaxKind kind) =>
        kind is SyntaxKind.MultiLineCommentTrivia or SyntaxKind.MultiLineDocumentationCommentTrivia;

    private static bool IsDirective(SyntaxKind kind) =>
        kind
            is SyntaxKind.IfDirectiveTrivia
                or SyntaxKind.ElseDirectiveTrivia
                or SyntaxKind.ElifDirectiveTrivia
                or SyntaxKind.EndIfDirectiveTrivia
                or SyntaxKind.LineDirectiveTrivia
                or SyntaxKind.ErrorDirectiveTrivia
                or SyntaxKind.WarningDirectiveTrivia
                or SyntaxKind.PragmaWarningDirectiveTrivia
                or SyntaxKind.PragmaChecksumDirectiveTrivia
                or SyntaxKind.DefineDirectiveTrivia
                or SyntaxKind.UndefDirectiveTrivia
                or SyntaxKind.NullableDirectiveTrivia;

    private static bool IsRegion(SyntaxKind kind) =>
        kind is SyntaxKind.RegionDirectiveTrivia or SyntaxKind.EndRegionDirectiveTrivia;

    private static Doc PrintTrailingTrivia(SyntaxToken node)
    {
        return PrintTrailingTrivia(node.TrailingTrivia);
    }

    private static Doc PrintTrailingTrivia(SyntaxTriviaList trailingTrivia)
    {
        var docs = new List<Doc>();
        foreach (var trivia in trailingTrivia)
        {
            if (trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
            {
                docs.Add(Doc.TrailingComment(trivia.ToString(), CommentType.SingleLine));
            }
            else if (trivia.Kind() == SyntaxKind.MultiLineCommentTrivia)
            {
                docs.Add(" ", Doc.TrailingComment(trivia.ToString(), CommentType.MultiLine));
            }
        }

        return docs.Count > 0 ? Doc.Concat(docs) : Doc.Null;
    }

    public static bool HasComments(SyntaxToken syntaxToken)
    {
        return syntaxToken.LeadingTrivia.Any(
                o => o.Kind() is not (SyntaxKind.WhitespaceTrivia or SyntaxKind.EndOfLineTrivia)
            )
            || syntaxToken.TrailingTrivia.Any(
                o => o.Kind() is not (SyntaxKind.WhitespaceTrivia or SyntaxKind.EndOfLineTrivia)
            );
    }
}
