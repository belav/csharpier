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

            return parts.Any() ? Concat(parts) : null;
        }

        // TODO 0 multiline comments need lots of testing, formatting is real weird
        private Doc PrintSyntaxToken(
            SyntaxToken syntaxToken,
            Doc afterTokenIfNoTrailing = null,
            Doc beforeTokenIfNoLeading = null)
        {
            if (syntaxToken.RawKind == 0)
            {
                return null;
            }

            var parts = new Parts();
            var leadingTrivia = this.PrintLeadingTrivia(syntaxToken);
            if (leadingTrivia != null)
            {
                parts.Push(leadingTrivia);
            }
            else
            {
                parts.Push(beforeTokenIfNoLeading);
            }
            parts.Push(syntaxToken.Text);
            var trailingTrivia = this.PrintTrailingTrivia(syntaxToken);
            if (trailingTrivia != null)
            {
                parts.Push(trailingTrivia);
            }
            else
            {
                parts.Push(afterTokenIfNoTrailing);
            }

            return Concat(parts);
        }

        private Doc PrintLeadingTrivia(SyntaxToken syntaxToken)
        {
            return this.PrintLeadingTrivia(syntaxToken.LeadingTrivia);
        }

        // TODO 1 probably ditch this, but leave it around for now
        private readonly Stack<bool> printNewLinesInLeadingTrivia = new();

        private Doc PrintLeadingTrivia(SyntaxTriviaList leadingTrivia)
        {
            var parts = new Parts();

            this.printNewLinesInLeadingTrivia.TryPeek(out var doNewLines);

            var hadDirective = false;
            for (var x = 0; x < leadingTrivia.Count; x++)
            {
                var trivia = leadingTrivia[x];

                if (doNewLines && trivia.Kind() == SyntaxKind.EndOfLineTrivia)
                {
                    SyntaxKind? kind = null;
                    if (x < leadingTrivia.Count - 1)
                    {
                        kind = leadingTrivia[x + 1].Kind();
                    }
                    // TODO 0 this may screw up with regions that aren't at the beginning of the line? should we deal with new lines/trivia between things differently??
                    if (
                        !kind.HasValue ||
                        kind == SyntaxKind.SingleLineCommentTrivia ||
                        kind == SyntaxKind.EndOfLineTrivia ||
                        kind == SyntaxKind.WhitespaceTrivia
                    )
                    {
                        parts.Push(HardLine);
                    }
                }
                if (
                    trivia.Kind() != SyntaxKind.EndOfLineTrivia &&
                    trivia.Kind() != SyntaxKind.WhitespaceTrivia
                )
                {
                    if (doNewLines)
                    {
                        doNewLines = false;
                        this.printNewLinesInLeadingTrivia.Pop();
                        this.printNewLinesInLeadingTrivia.Push(false);
                    }
                }
                if (
                    trivia.Kind() == SyntaxKind.SingleLineCommentTrivia ||
                    trivia.Kind() == SyntaxKind.SingleLineDocumentationCommentTrivia
                )
                {
                    parts.Push(
                        LeadingComment(
                            trivia.ToFullString().TrimEnd('\n', '\r'),
                            CommentType.SingleLine));
                }
                else if (
                    trivia.Kind() == SyntaxKind.MultiLineCommentTrivia ||
                    trivia.Kind() == SyntaxKind.MultiLineDocumentationCommentTrivia
                )
                {
                    parts.Push(
                        LeadingComment(
                            trivia.ToFullString().TrimEnd('\n', '\r'),
                            CommentType.MultiLine));
                }
                else if (trivia.Kind() == SyntaxKind.DisabledTextTrivia)
                {
                    parts.Push(
                        LiteralLine,
                        trivia.ToString().TrimEnd('\n', '\r'));
                }
                else if (
                    trivia.Kind() == SyntaxKind.IfDirectiveTrivia ||
                    trivia.Kind() == SyntaxKind.ElseDirectiveTrivia ||
                    trivia.Kind() == SyntaxKind.ElifDirectiveTrivia ||
                    trivia.Kind() == SyntaxKind.EndIfDirectiveTrivia ||
                    trivia.Kind() == SyntaxKind.LineDirectiveTrivia ||
                    trivia.Kind() == SyntaxKind.ErrorDirectiveTrivia ||
                    trivia.Kind() == SyntaxKind.WarningDirectiveTrivia ||
                    trivia.Kind() == SyntaxKind.PragmaWarningDirectiveTrivia ||
                    trivia.Kind() == SyntaxKind.PragmaChecksumDirectiveTrivia ||
                    trivia.Kind() == SyntaxKind.DefineDirectiveTrivia ||
                    trivia.Kind() == SyntaxKind.UndefDirectiveTrivia ||
                    trivia.Kind() == SyntaxKind.NullableDirectiveTrivia
                )
                {
                    hadDirective = true;
                    parts.Push(LiteralLine, trivia.ToString());
                }
                else if (
                    trivia.Kind() == SyntaxKind.RegionDirectiveTrivia ||
                    trivia.Kind() == SyntaxKind.EndRegionDirectiveTrivia
                )
                {
                    var triviaText = trivia.ToString();
                    if (
                        x > 0 &&
                        leadingTrivia[x - 1].Kind() == SyntaxKind.WhitespaceTrivia
                    )
                    {
                        triviaText = leadingTrivia[x - 1] + triviaText;
                    }

                    hadDirective = true;
                    parts.Push(LiteralLine, triviaText);
                }
            }

            if (hadDirective)
            {
                parts.Push(HardLine);
            }

            return parts.Count > 0 ? Concat(parts) : null;
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
                            CommentType.SingleLine));
                }
                else if (trivia.Kind() == SyntaxKind.MultiLineCommentTrivia)
                {
                    parts.Push(" ", trivia.ToString(), Line);
                }
            }

            return parts.Count > 0 ? Concat(parts) : null;
        }
    }
}
