using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintExtraNewLines(CSharpSyntaxNode node)
        {
            var parts = new Parts();
            foreach (var leadingTrivia in node.GetLeadingTrivia())
            {
                if (leadingTrivia.Kind() == SyntaxKind.EndOfLineTrivia)
                {
                    parts.Push(HardLine);
                }
                else if (leadingTrivia.Kind() != SyntaxKind.WhitespaceTrivia)
                {
                    break;
                }
            }

            return parts.Any() ? Concat(parts) : Doc.Null;
        }

        // TODO 0 multiline comments need lots of testing, formatting is real weird
        private Doc PrintSyntaxToken(
            SyntaxToken syntaxToken,
            Doc? afterTokenIfNoTrailing = null,
            Doc? beforeTokenIfNoLeading = null)
        {
            if (syntaxToken.RawKind == 0)
            {
                return Doc.Null;
            }

            var parts = new Parts();
            var leadingTrivia = this.PrintLeadingTrivia(syntaxToken);
            if (leadingTrivia != Doc.Null)
            {
                parts.Push(leadingTrivia);
            }
            else if (beforeTokenIfNoLeading != null)
            {
                parts.Push(beforeTokenIfNoLeading);
            }
            parts.Push(syntaxToken.Text);
            var trailingTrivia = this.PrintTrailingTrivia(syntaxToken);
            if (trailingTrivia != Doc.Null)
            {
                parts.Push(trailingTrivia);
            }
            else if (afterTokenIfNoTrailing != null)
            {
                parts.Push(afterTokenIfNoTrailing);
            }

            return Concat(parts);
        }

        private Doc PrintLeadingTrivia(SyntaxToken syntaxToken)
        {
            return this.PrintLeadingTrivia(syntaxToken.LeadingTrivia);
        }

        private Doc PrintLeadingTrivia(SyntaxTriviaList leadingTrivia)
        {
            var parts = new Parts();

            // we don't print any new lines until we run into a comment or directive, the PrintNewLines method takes care of printing the initial new lines for a given node
            // we also print double HardLines in some cases with directives. That's because a LiteralLine currently trims the previous new line it finds
            // If it doesn't, we end up adding extra lines in some situations
            // namespace Namespace
            // {                   - HardLine                               - if we didn't trim this HardLine, then we'd end up inserting a blank line between this and #pragma
            // #pragma             - LiteralLine, #pragma                   
            // vs
            // #region Region      - LiteralLine, #region, HardLine         - we end each directive with a hardline to ensure we get a double hardline in this situation
            //                     - HardLine                               - so the literal line trims this hardline, but we retain the newline between the region directives
            // #region Nested      - LiteralLine, #region, HardLine         - because of the double hardline.
            var printNewLines = false;

            for (var x = 0; x < leadingTrivia.Count; x++)
            {
                var trivia = leadingTrivia[x];

                if (
                    printNewLines
                    && trivia.Kind() == SyntaxKind.EndOfLineTrivia
                )
                {
                    parts.Push(HardLine);
                }
                if (
                    trivia.Kind() != SyntaxKind.EndOfLineTrivia
                    && trivia.Kind() != SyntaxKind.WhitespaceTrivia
                )
                {
                    printNewLines = true;
                }
                if (
                    trivia.Kind() == SyntaxKind.SingleLineCommentTrivia
                    || trivia.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia
                )
                {
                    parts.Push(
                        LeadingComment(
                            trivia.ToFullString().TrimEnd('\n', '\r'),
                            CommentType.SingleLine
                        )
                    );
                }
                else if (
                    trivia.Kind() == SyntaxKind.MultiLineCommentTrivia
                    || trivia.Kind() == SyntaxKind.MultiLineDocumentationCommentTrivia
                )
                {
                    parts.Push(
                        LeadingComment(
                            trivia.ToFullString().TrimEnd('\n', '\r'),
                            CommentType.MultiLine
                        )
                    );
                }
                else if (trivia.Kind() == SyntaxKind.DisabledTextTrivia)
                {
                    parts.Push(
                        LiteralLine,
                        trivia.ToString().TrimEnd('\n', '\r')
                    );
                }
                else if (
                    trivia.Kind() == SyntaxKind.IfDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.ElseDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.ElifDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.EndIfDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.LineDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.ErrorDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.WarningDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.PragmaWarningDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.PragmaChecksumDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.DefineDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.UndefDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.NullableDirectiveTrivia
                )
                {
                    parts.Push(LiteralLine, trivia.ToString(), HardLine);
                }
                else if (
                    trivia.Kind() == SyntaxKind.RegionDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.EndRegionDirectiveTrivia
                )
                {
                    var triviaText = trivia.ToString();
                    if (
                        x > 0
                        && leadingTrivia[
                            x - 1
                        ].Kind() == SyntaxKind.WhitespaceTrivia
                    )
                    {
                        triviaText = leadingTrivia[x - 1] + triviaText;
                    }

                    parts.Push(LiteralLine, triviaText, HardLine);
                }
            }

            return parts.Count > 0 ? Concat(parts) : Doc.Null;
        }

        private Doc PrintTrailingTrivia(SyntaxToken node)
        {
            return this.PrintTrailingTrivia(node.TrailingTrivia);
        }

        private Doc PrintTrailingTrivia(SyntaxTriviaList trailingTrivia)
        {
            var parts = new Parts();
            foreach (var trivia in trailingTrivia)
            {
                if (trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
                {
                    parts.Push(
                        TrailingComment(
                            trivia.ToString(),
                            CommentType.SingleLine
                        )
                    );
                }
                else if (trivia.Kind() == SyntaxKind.MultiLineCommentTrivia)
                {
                    parts.Push(
                        " ",
                        TrailingComment(
                            trivia.ToString(),
                            CommentType.MultiLine
                        )
                    );
                }
            }

            return parts.Count > 0 ? Concat(parts) : Doc.Null;
        }
    }
}
