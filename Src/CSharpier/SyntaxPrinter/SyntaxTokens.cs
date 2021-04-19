using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                        Docs.Trim,
                        trivia.ToString().TrimEnd('\n', '\r'),
                        Docs.HardLine
                    );
                }
                else if (IsDirective(kind))
                {
                    // handles the case of a method that only contains #if DEBUG
                    if (
                        kind == SyntaxKind.IfDirectiveTrivia
                        && trivia.Token.Kind() == SyntaxKind.CloseBraceToken
                        && trivia.Token.Parent is BlockSyntax blockSyntax
                        && blockSyntax.Statements.Count == 0
                    ) {
                        docs.Add(Docs.HardLineIfNoPreviousLine);
                    }
                    docs.Add(
                        Docs.HardLineIfNoPreviousLine,
                        Docs.Trim,
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

                    docs.Add(
                        Docs.HardLineIfNoPreviousLine,
                        Docs.Trim,
                        triviaText,
                        Docs.HardLine
                    );
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
