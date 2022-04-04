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
        if (syntaxToken.IsKind(SyntaxKind.None))
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
            (syntaxToken.IsKind(SyntaxKind.StringLiteralToken) && syntaxToken.Text.StartsWith("@"))
            || (
                syntaxToken.IsKind(SyntaxKind.InterpolatedStringTextToken)
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
        var isClosingBrace = syntaxToken.IsKind(SyntaxKind.CloseBraceToken);

        Doc extraNewLines = Doc.Null;

        if (isClosingBrace && syntaxToken.LeadingTrivia.Any(o => o.IsDirective || o.IsComment()))
        {
            extraNewLines = ExtraNewLines.Print(syntaxToken.LeadingTrivia);
        }

        var printedTrivia = PrivatePrintLeadingTrivia(
            syntaxToken.LeadingTrivia,
            skipLastHardline: isClosingBrace
        );

        return isClosingBrace && (printedTrivia != Doc.Null || extraNewLines != Doc.Null)
          ? Doc.Concat(
                extraNewLines,
                Doc.IndentIf(printedTrivia != Doc.Null, printedTrivia),
                Doc.HardLine
            )
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
            var kind = trivia.RawKind;

            if (printNewLines && kind == (int)SyntaxKind.EndOfLineTrivia)
            {
                docs.Add(Doc.HardLineSkipBreakIfFirstInGroup);
            }
            if (kind != (int)SyntaxKind.EndOfLineTrivia && kind != (int)SyntaxKind.WhitespaceTrivia)
            {
                printNewLines = true;
            }

            void AddLeadingComment(CommentType commentType)
            {
                var comment = trivia.ToFullString().TrimEnd('\n', '\r');
                if (
                    commentType == CommentType.MultiLine
                    && x > 0
                    && leadingTrivia[x - 1].IsKind(SyntaxKind.WhitespaceTrivia)
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
                    kind == (int)SyntaxKind.SingleLineDocumentationCommentTrivia
                      ? Doc.HardLineSkipBreakIfFirstInGroup
                      : Doc.Null
                );
            }
            else if (IsMultiLineComment(kind))
            {
                AddLeadingComment(CommentType.MultiLine);
            }
            else if (kind == (int)SyntaxKind.DisabledTextTrivia)
            {
                docs.Add(Doc.Trim, trivia.ToString());
            }
            else if (IsDirective(kind) || IsRegion(kind))
            {
                var triviaText = trivia.ToString();
                if (
                    IsRegion(kind)
                    && x > 0
                    && leadingTrivia[x - 1].IsKind(SyntaxKind.WhitespaceTrivia)
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

    private static bool IsSingleLineComment(int kind) =>
        kind
            is (int)SyntaxKind.SingleLineDocumentationCommentTrivia
                or (int)SyntaxKind.SingleLineCommentTrivia;

    private static bool IsMultiLineComment(int kind) =>
        kind
            is (int)SyntaxKind.MultiLineCommentTrivia
                or (int)SyntaxKind.MultiLineDocumentationCommentTrivia;

    private static bool IsDirective(int kind) =>
        kind
            is (int)SyntaxKind.IfDirectiveTrivia
                or (int)SyntaxKind.ElseDirectiveTrivia
                or (int)SyntaxKind.ElifDirectiveTrivia
                or (int)SyntaxKind.EndIfDirectiveTrivia
                or (int)SyntaxKind.LineDirectiveTrivia
                or (int)SyntaxKind.ErrorDirectiveTrivia
                or (int)SyntaxKind.WarningDirectiveTrivia
                or (int)SyntaxKind.PragmaWarningDirectiveTrivia
                or (int)SyntaxKind.PragmaChecksumDirectiveTrivia
                or (int)SyntaxKind.DefineDirectiveTrivia
                or (int)SyntaxKind.UndefDirectiveTrivia
                or (int)SyntaxKind.NullableDirectiveTrivia;

    private static bool IsRegion(int kind) =>
        kind is (int)SyntaxKind.RegionDirectiveTrivia or (int)SyntaxKind.EndRegionDirectiveTrivia;

    private static Doc PrintTrailingTrivia(SyntaxToken node)
    {
        return PrintTrailingTrivia(node.TrailingTrivia);
    }

    private static Doc PrintTrailingTrivia(SyntaxTriviaList trailingTrivia)
    {
        var docs = new List<Doc>();
        foreach (var trivia in trailingTrivia)
        {
            if (trivia.IsKind(SyntaxKind.SingleLineCommentTrivia))
            {
                docs.Add(Doc.TrailingComment(trivia.ToString(), CommentType.SingleLine));
            }
            else if (trivia.IsKind(SyntaxKind.MultiLineCommentTrivia))
            {
                docs.Add(" ", Doc.TrailingComment(trivia.ToString(), CommentType.MultiLine));
            }
        }

        return docs.Count > 0 ? Doc.Concat(docs) : Doc.Null;
    }

    public static bool HasComments(SyntaxToken syntaxToken)
    {
        return syntaxToken.LeadingTrivia.Any(
                o =>
                    !(o.IsKind(SyntaxKind.WhitespaceTrivia) || o.IsKind(SyntaxKind.EndOfLineTrivia))
            )
            || syntaxToken.TrailingTrivia.Any(
                o =>
                    !(o.IsKind(SyntaxKind.WhitespaceTrivia) || o.IsKind(SyntaxKind.EndOfLineTrivia))
            );
    }
}
