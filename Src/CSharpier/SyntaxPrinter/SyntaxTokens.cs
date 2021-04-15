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
            Doc? afterTokenIfNoTrailing = null
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
                var kind = trivia.Kind();

                if (printNewLines && kind == SyntaxKind.EndOfLineTrivia)
                {
                    docs.Add(Docs.HardLine);
                }
                if (
                    kind != SyntaxKind.EndOfLineTrivia
                    && kind != SyntaxKind.WhitespaceTrivia
                ) {
                    printNewLines = true;
                }
                if (IsSingleLineComment(kind))
                {
                    docs.Add(
                        Docs.LeadingComment(
                            trivia.ToFullString().TrimEnd('\n', '\r'),
                            CommentType.SingleLine
                        ),
                        kind == SyntaxKind.SingleLineDocumentationCommentTrivia
                            ? Docs.HardLine
                            : Doc.Null
                    );
                }
                else if (IsMultiLineComment(kind))
                {
                    docs.Add(
                        Docs.LeadingComment(
                            trivia.ToFullString().TrimEnd('\n', '\r'),
                            CommentType.MultiLine
                        )
                    );
                }
                else if (kind == SyntaxKind.DisabledTextTrivia)
                {
                    docs.Add(
                        Docs.LiteralLine,
                        trivia.ToString().TrimEnd('\n', '\r')
                    );
                }
                else if (IsDirective(kind))
                {
                    docs.Add(
                        Docs.LiteralLine,
                        trivia.ToString(),
                        Docs.HardLine
                    );
                }
                else if (IsRegion(kind))
                {
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

        private static bool IsSingleLineComment(SyntaxKind kind) =>
            kind == SyntaxKind.SingleLineDocumentationCommentTrivia
            || kind == SyntaxKind.SingleLineCommentTrivia;

        private static bool IsMultiLineComment(SyntaxKind kind) =>
            kind == SyntaxKind.MultiLineCommentTrivia
            || kind == SyntaxKind.MultiLineDocumentationCommentTrivia;

        private static bool IsDirective(SyntaxKind kind) =>
            kind == SyntaxKind.IfDirectiveTrivia
            || kind == SyntaxKind.ElseDirectiveTrivia
            || kind == SyntaxKind.ElifDirectiveTrivia
            || kind == SyntaxKind.EndIfDirectiveTrivia
            || kind == SyntaxKind.LineDirectiveTrivia
            || kind == SyntaxKind.ErrorDirectiveTrivia
            || kind == SyntaxKind.WarningDirectiveTrivia
            || kind == SyntaxKind.PragmaWarningDirectiveTrivia
            || kind == SyntaxKind.PragmaChecksumDirectiveTrivia
            || kind == SyntaxKind.DefineDirectiveTrivia
            || kind == SyntaxKind.UndefDirectiveTrivia
            || kind == SyntaxKind.NullableDirectiveTrivia;

        private static bool IsRegion(SyntaxKind kind) =>
            kind == SyntaxKind.RegionDirectiveTrivia
            || kind == SyntaxKind.EndRegionDirectiveTrivia;

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
