using System.Text.RegularExpressions;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class Token
{
    public static Doc PrintWithoutLeadingTrivia(SyntaxToken syntaxToken, PrintingContext context)
    {
        return PrintSyntaxToken(syntaxToken, context, skipLeadingTrivia: true);
    }

    public static Doc PrintWithoutTrailingTrivia(SyntaxToken syntaxToken, PrintingContext context)
    {
        return PrintSyntaxToken(syntaxToken, context, skipTrailingTrivia: true);
    }

    public static Doc Print(SyntaxToken syntaxToken, PrintingContext context)
    {
        return PrintSyntaxToken(syntaxToken, context);
    }

    public static Doc PrintWithSuffix(
        SyntaxToken syntaxToken,
        Doc suffixDoc,
        PrintingContext context,
        bool skipLeadingTrivia = false
    )
    {
        return PrintSyntaxToken(syntaxToken, context, suffixDoc, skipLeadingTrivia);
    }

    internal static readonly string[] lineSeparators = ["\r\n", "\r", "\n"];

    private static Doc PrintSyntaxToken(
        SyntaxToken syntaxToken,
        PrintingContext context,
        Doc? suffixDoc = null,
        bool skipLeadingTrivia = false,
        bool skipTrailingTrivia = false
    )
    {
        if (syntaxToken.RawSyntaxKind() == SyntaxKind.None)
        {
            return Doc.Null;
        }

        var docs = new ValueListBuilder<Doc>([null, null, null, null, null, null, null, null]);

        if (!skipLeadingTrivia)
        {
            var leadingTrivia = PrintLeadingTrivia(syntaxToken, context);
            if (leadingTrivia != Doc.Null)
            {
                docs.Append(leadingTrivia);
            }
        }

        if (
            (
                syntaxToken.RawSyntaxKind() == SyntaxKind.StringLiteralToken
                && syntaxToken.Text.StartsWith('@')
            )
            || (
                syntaxToken.RawSyntaxKind() == SyntaxKind.InterpolatedStringTextToken
                && syntaxToken.Parent!.Parent
                    is InterpolatedStringExpressionSyntax
                    {
                        StringStartToken.RawKind: (int)
                            SyntaxKind.InterpolatedVerbatimStringStartToken
                    }
            )
        )
        {
            var lines = syntaxToken.Text.Replace("\r", string.Empty).Split('\n');
            docs.Append(Doc.Join(Doc.LiteralLine, lines.Select(o => new StringDoc(o))));
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

            docs.Append(Doc.IndentIf(!hasArgumentParent, Doc.Concat(contents)));
        }
        else if (
            syntaxToken.RawSyntaxKind()
            is SyntaxKind.InterpolatedMultiLineRawStringStartToken
                or SyntaxKind.InterpolatedRawStringEndToken
        )
        {
            docs.Append(syntaxToken.Text.Trim());
        }
        else
        {
            docs.Append(StringDoc.Create(syntaxToken));
        }

        if (!skipTrailingTrivia)
        {
            var trailingTrivia = PrintTrailingTrivia(syntaxToken);
            if (trailingTrivia != Doc.Null)
            {
                if (
                    context.State.TrailingComma is not null
                    && Enumerable.FirstOrDefault(syntaxToken.TrailingTrivia, o => o.IsComment())
                        == context.State.TrailingComma.TrailingComment
                )
                {
                    docs.Append(context.State.TrailingComma.PrintedTrailingComma);
                    context.State.MovedTrailingTrivia = true;
                    context.State.TrailingComma = null;
                }

                docs.Append(trailingTrivia);
            }
        }

        if (suffixDoc != null)
        {
            docs.Append(suffixDoc);
        }

        var returnDoc = Doc.Concat(ref docs);
        docs.Dispose();

        return returnDoc;
    }

    public static Doc PrintLeadingTrivia(SyntaxToken syntaxToken, PrintingContext context)
    {
        if (context.State.SkipNextLeadingTrivia)
        {
            context.State.SkipNextLeadingTrivia = false;
            return Doc.Null;
        }

        var isClosingBrace =
            syntaxToken.RawSyntaxKind() == SyntaxKind.CloseBraceToken
            || syntaxToken.Parent is CollectionExpressionSyntax
                && syntaxToken.RawSyntaxKind() == SyntaxKind.CloseBracketToken;

        var printedTrivia = PrivatePrintLeadingTrivia(
            syntaxToken.LeadingTrivia,
            context,
            skipLastHardline: isClosingBrace
        );

        var hasDirective = Enumerable.Any(syntaxToken.LeadingTrivia, o => o.IsDirective);

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

        if (hasDirective || Enumerable.Any(syntaxToken.LeadingTrivia, o => o.IsComment()))
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

    public static Doc PrintLeadingTrivia(SyntaxTriviaList leadingTrivia, PrintingContext context)
    {
        return PrivatePrintLeadingTrivia(leadingTrivia, context);
    }

    public static Doc PrintLeadingTriviaWithNewLines(
        SyntaxTriviaList leadingTrivia,
        PrintingContext context
    )
    {
        return PrivatePrintLeadingTrivia(leadingTrivia, context, includeInitialNewLines: true);
    }

    private static Doc PrivatePrintLeadingTrivia(
        SyntaxTriviaList leadingTrivia,
        PrintingContext context,
        bool includeInitialNewLines = false,
        bool skipLastHardline = false
    )
    {
        if (leadingTrivia.Count == 0)
        {
            return Doc.Null;
        }

        var docs = new ValueListBuilder<Doc>([null, null, null, null, null, null, null, null]);

        // we don't print any new lines until we run into a comment or directive
        // the PrintExtraNewLines method takes care of printing the initial new lines for a given node
        var printNewLines = includeInitialNewLines;

        for (var x = 0; x < leadingTrivia.Count; x++)
        {
            var trivia = leadingTrivia[x];
            var kind = trivia.RawSyntaxKind();

            if (printNewLines && kind == SyntaxKind.EndOfLineTrivia)
            {
                if (docs.Length > 0 && docs[^1] == Doc.HardLineSkipBreakIfFirstInGroup)
                {
                    printNewLines = false;
                }

                if (
                    !(
                        docs.Length > 1
                        && docs[^1] == Doc.HardLineSkipBreakIfFirstInGroup
                        && docs[^2] is LeadingComment { Type: CommentType.SingleLine }
                    )
                )
                {
                    docs.Append(Doc.HardLineSkipBreakIfFirstInGroup);
                }
            }
            if (kind is not (SyntaxKind.EndOfLineTrivia or SyntaxKind.WhitespaceTrivia))
            {
                printNewLines = true;
            }

            void AddLeadingComment(CommentType commentType, ref ValueListBuilder<Doc> docs)
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

                docs.Append(Doc.LeadingComment(comment, commentType));
            }

            if (IsSingleLineComment(kind))
            {
                AddLeadingComment(CommentType.SingleLine, ref docs);
                docs.Append(
                    kind == SyntaxKind.SingleLineDocumentationCommentTrivia
                        ? Doc.HardLineSkipBreakIfFirstInGroup
                        : Doc.Null
                );
            }
            else if (IsMultiLineComment(kind))
            {
                AddLeadingComment(CommentType.MultiLine, ref docs);
            }
            else if (kind == SyntaxKind.DisabledTextTrivia)
            {
                docs.Append(Doc.Trim, trivia.ToString());
            }
            else if (IsRegion(kind))
            {
                var triviaText = trivia.ToString();
                docs.Append(Doc.HardLineIfNoPreviousLine);
                docs.Append(Doc.Trim);
                docs.Append(
                    kind == SyntaxKind.RegionDirectiveTrivia
                        ? Doc.BeginRegion(triviaText)
                        : Doc.EndRegion(triviaText)
                );
                docs.Append(Doc.HardLine);
            }
            else if (trivia.IsDirective)
            {
                var triviaText = trivia.ToString();

                docs.Append(
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
                        docs.Append(Doc.HardLineSkipBreakIfFirstInGroup);
                    }
                    printNewLines = false;
                }
            }
        }

        while (skipLastHardline && docs.Length != 0 && docs[^1] is HardLine or NullDoc)
        {
            docs.Pop();
        }

        if (context.State.NextTriviaNeedsLine)
        {
            if (
                Enumerable.Any(
                    leadingTrivia,
                    o => o.RawSyntaxKind() is SyntaxKind.IfDirectiveTrivia
                )
            )
            {
                docs.Insert(0, Doc.HardLineSkipBreakIfFirstInGroup);
            }
            else
            {
                var index = docs.Length - 1;
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
                    index + 2 >= docs.Length
                    || !(docs[index + 1] is HardLine && docs[index + 2] is HardLine)
                )
                {
                    docs.Insert(index + 1, Doc.HardLineSkipBreakIfFirstInGroup);
                }
            }
            context.State.NextTriviaNeedsLine = false;
        }

        return Doc.Concat(ref docs);
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

    private static Doc PrintTrailingTrivia(in SyntaxTriviaList trailingTrivia)
    {
        if (trailingTrivia.Count == 0)
        {
            return Doc.Null;
        }

        var docs = new ValueListBuilder<Doc>([null, null, null, null, null, null, null, null]);
        foreach (var trivia in trailingTrivia)
        {
            if (trivia.RawSyntaxKind() == SyntaxKind.SingleLineCommentTrivia)
            {
                docs.Append(Doc.TrailingComment(trivia.ToString(), CommentType.SingleLine));
            }
            else if (trivia.RawSyntaxKind() == SyntaxKind.MultiLineCommentTrivia)
            {
                docs.Append(" ", Doc.TrailingComment(trivia.ToString(), CommentType.MultiLine));
            }
        }

        var returnDoc = Doc.Concat(ref docs);
        docs.Dispose();

        return returnDoc;
    }

    public static bool HasComments(SyntaxToken syntaxToken)
    {
        return Enumerable.Any(
                syntaxToken.LeadingTrivia,
                o =>
                    o.RawSyntaxKind()
                        is not (SyntaxKind.WhitespaceTrivia or SyntaxKind.EndOfLineTrivia)
            )
            || Enumerable.Any(
                syntaxToken.TrailingTrivia,
                o =>
                    o.RawSyntaxKind()
                        is not (SyntaxKind.WhitespaceTrivia or SyntaxKind.EndOfLineTrivia)
            );
    }

    public static bool HasLeadingCommentMatching(SyntaxNode node, Regex regex)
    {
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var o in node.GetLeadingTrivia())
        {
            if (
                o.RawSyntaxKind() is SyntaxKind.SingleLineCommentTrivia
                && regex.IsMatch(o.ToString())
            )
            {
                return true;
            }
        }

        return false;
    }

    public static bool HasLeadingCommentMatching(SyntaxToken token, Regex regex)
    {
        return Enumerable.Any(
            token.LeadingTrivia,
            o =>
                o.RawSyntaxKind() is SyntaxKind.SingleLineCommentTrivia
                && regex.IsMatch(o.ToString())
        );
    }
}
