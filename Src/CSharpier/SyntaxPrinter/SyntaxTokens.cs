using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpier.SyntaxPrinter
{
    public class SyntaxTokens
    {
        public static Doc Print(SyntaxToken syntaxToken)
        {
            return PrintSyntaxToken(syntaxToken);
        }

        // TODO long term make this private, and expose methods for Print, PrintWithAfterToken, PrintWithBeforeToken (not the real names, we can do better!)
        // actually if we change how comments/directives print, maybe we don't need the before/after tokens
        public static Doc PrintSyntaxToken(
            SyntaxToken syntaxToken,
            Doc? afterTokenIfNoTrailing = null,
            Doc? beforeTokenIfNoLeading = null
        ) {
            if (syntaxToken.RawKind == 0)
            {
                return Doc.Null;
            }

            var docs = new List<Doc>();
            var leadingTrivia = PrintLeadingTrivia(syntaxToken);
            if (leadingTrivia != Doc.Null)
            {
                docs.Add(leadingTrivia);
            }
            else if (beforeTokenIfNoLeading != null)
            {
                docs.Add(beforeTokenIfNoLeading);
            }
            docs.Add(syntaxToken.Text);
            var trailingTrivia = PrintTrailingTrivia(syntaxToken);
            if (trailingTrivia != Doc.Null)
            {
                docs.Add(trailingTrivia);
            }
            else if (afterTokenIfNoTrailing != null)
            {
                docs.Add(afterTokenIfNoTrailing);
            }

            return Docs.Concat(docs);
        }

        private static Doc PrintLeadingTrivia(SyntaxToken syntaxToken)
        {
            var indentTrivia = syntaxToken.Kind() == SyntaxKind.CloseBraceToken;

            var printedTrivia = PrintLeadingTrivia(
                syntaxToken.LeadingTrivia,
                skipLastHardline: indentTrivia
            );

            return indentTrivia && printedTrivia != Doc.Null
                ? Docs.Concat(Docs.Indent(printedTrivia), Docs.HardLine)
                : printedTrivia;
        }

        // LiteralLines are a little odd because they trim any new line immediately before them. The reason is as follows.
        // namespace Namespace
        // {                   - HardLine                           - if the LiteralLine below didn't trim this HardLine, then we'd end up inserting a blank line between this and #pragma
        // #pragma             - LiteralLine, #pragma               - The HardLine above could come from a number of different PrintNode methods
        //
        // #region Region      - LiteralLine, #region, HardLine     - we end each directive with a hardLine to ensure we get a double hardLine in this situation
        //                     - HardLine                           - this hardLine is trimmed by the literalLine below, but the extra hardline above ensures
        // #region Nested      - LiteralLine, #region, HardLine     - we still keep the blank line between the regions
        //
        // #pragma             - LiteralLine, #pragma, HardLine
        // #pragma             - LiteralLine, #pragma, Hardline     - And this LiteralLine trims the extra HardLine above to ensure we don't get an extra blank line
        public static Doc PrintLeadingTrivia( // make this private eventually, figure out if we can ditch the special case for CompilationUnitSyntax
            SyntaxTriviaList leadingTrivia,
            bool includeInitialNewLines = false,
            bool skipLastHardline = false
        ) {
            var docs = new List<Doc>();

            // we don't print any new lines until we run into a comment or directive
            // the PrintExtraNewLines method takes care of printing the initial new lines for a given node
            var printNewLines = includeInitialNewLines;

            for (var x = 0; x < leadingTrivia.Count; x++)
            {
                var trivia = leadingTrivia[x];

                if (
                    printNewLines && trivia.Kind() == SyntaxKind.EndOfLineTrivia
                ) {
                    docs.Add(Docs.HardLine);
                }
                if (
                    trivia.Kind() != SyntaxKind.EndOfLineTrivia
                    && trivia.Kind() != SyntaxKind.WhitespaceTrivia
                ) {
                    printNewLines = true;
                }
                if (
                    trivia.Kind() == SyntaxKind.SingleLineCommentTrivia
                    || trivia.Kind()
                    == SyntaxKind.SingleLineDocumentationCommentTrivia
                ) {
                    docs.Add(
                        Docs.LeadingComment(
                            trivia.ToFullString().TrimEnd('\n', '\r'),
                            CommentType.SingleLine
                        ),
                        trivia.Kind()
                            == SyntaxKind.SingleLineDocumentationCommentTrivia
                            ? Docs.HardLine
                            : Doc.Null
                    );
                }
                else if (
                    trivia.Kind() == SyntaxKind.MultiLineCommentTrivia
                    || trivia.Kind()
                    == SyntaxKind.MultiLineDocumentationCommentTrivia
                ) {
                    docs.Add(
                        Docs.LeadingComment(
                            trivia.ToFullString().TrimEnd('\n', '\r'),
                            CommentType.MultiLine
                        )
                    );
                }
                else if (trivia.Kind() == SyntaxKind.DisabledTextTrivia)
                {
                    docs.Add(
                        Docs.LiteralLine,
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
                ) {
                    docs.Add(
                        Docs.LiteralLine,
                        trivia.ToString(),
                        Docs.HardLine
                    );
                }
                else if (
                    trivia.Kind() == SyntaxKind.RegionDirectiveTrivia
                    || trivia.Kind() == SyntaxKind.EndRegionDirectiveTrivia
                ) {
                    var triviaText = trivia.ToString();
                    if (
                        x > 0
                        && leadingTrivia[x - 1].Kind()
                        == SyntaxKind.WhitespaceTrivia
                    ) {
                        triviaText = leadingTrivia[x - 1] + triviaText;
                    }

                    docs.Add(Docs.LiteralLine, triviaText, Docs.HardLine);
                }
            }

            if (skipLastHardline && docs.Any() && docs.Last() is HardLine)
            {
                docs.RemoveAt(docs.Count - 1);
            }

            return docs.Count > 0 ? Docs.Concat(docs) : Doc.Null;
        }

        private static Doc PrintTrailingTrivia(SyntaxToken node)
        {
            return PrintTrailingTrivia(node.TrailingTrivia);
        }

        private static Doc PrintTrailingTrivia(
            SyntaxTriviaList trailingTrivia
        ) {
            var docs = new List<Doc>();
            foreach (var trivia in trailingTrivia)
            {
                if (trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
                {
                    docs.Add(
                        Docs.TrailingComment(
                            trivia.ToString(),
                            CommentType.SingleLine
                        )
                    );
                }
                else if (trivia.Kind() == SyntaxKind.MultiLineCommentTrivia)
                {
                    docs.Add(
                        " ",
                        Docs.TrailingComment(
                            trivia.ToString(),
                            CommentType.MultiLine
                        )
                    );
                }
            }

            return docs.Count > 0 ? Docs.Concat(docs) : Docs.Null;
        }
    }
}
