using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter
{
    public class Token
    {
        public static Doc PrintWithoutLeadingTrivia(SyntaxToken syntaxToken)
        {
            return PrintSyntaxToken(syntaxToken, null, skipLeadingTrivia: true);
        }

        public static Doc Print(SyntaxToken syntaxToken)
        {
            return PrintSyntaxToken(syntaxToken);
        }

        // TODO partial kill
        public static Doc Print(SyntaxToken syntaxToken, Doc suffixDoc)
        {
            return PrintWithSuffix(syntaxToken, suffixDoc);
        }

        public static Doc PrintWithSuffix(
            SyntaxToken syntaxToken,
            Doc suffixDoc
        ) {
            return PrintSyntaxToken(syntaxToken, suffixDoc);
        }

        // TODO long term make this private, and expose methods for Print, PrintWithAfterToken, PrintWithBeforeToken (not the real names, we can do better!)
        // actually if we change how comments/directives print, maybe we don't need the before/after tokens
        public static Doc PrintSyntaxToken(
            SyntaxToken syntaxToken,
            Doc? suffixDoc = null,
            bool skipLeadingTrivia = false
        ) {
            if (syntaxToken.RawKind == 0)
            {
                return Doc.Null;
            }

            var docs = new List<Doc>();
            if (!skipLeadingTrivia)
            {
                var leadingTrivia = PrintLeadingTrivia(syntaxToken);
                if (leadingTrivia != Doc.Null)
                {
                    docs.Add(leadingTrivia);
                }
            }
            docs.Add(syntaxToken.Text);
            var trailingTrivia = PrintTrailingTrivia(syntaxToken);
            if (trailingTrivia != Doc.Null)
            {
                docs.Add(trailingTrivia);
            }

            if (suffixDoc != null)
            {
                docs.Add(suffixDoc);
            }

            return Doc.Concat(docs);
        }

        public static Doc PrintLeadingTrivia(SyntaxToken syntaxToken)
        {
            var indentTrivia = syntaxToken.Kind() == SyntaxKind.CloseBraceToken;

            var printedTrivia = PrintLeadingTrivia(
                syntaxToken.LeadingTrivia,
                skipLastHardline: indentTrivia
            );

            return indentTrivia && printedTrivia != Doc.Null
                ? Doc.Concat(Doc.Indent(printedTrivia), Doc.HardLine)
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
                    docs.Add(Doc.HardLine);
                }
                if (
                    kind != SyntaxKind.EndOfLineTrivia &&
                    kind != SyntaxKind.WhitespaceTrivia
                ) {
                    printNewLines = true;
                }
                if (IsSingleLineComment(kind))
                {
                    docs.Add(
                        Doc.LeadingComment(
                            trivia.ToFullString().TrimEnd('\n', '\r'),
                            CommentType.SingleLine
                        ),
                        kind == SyntaxKind.SingleLineDocumentationCommentTrivia
                            ? Doc.HardLine
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
                    docs.Add(
                        Doc.Trim,
                        trivia.ToString().TrimEnd('\n', '\r'),
                        Doc.HardLine
                    );
                }
                else if (IsDirective(kind))
                {
                    // handles the case of a method that only contains #if DEBUG
                    if (
                        kind == SyntaxKind.IfDirectiveTrivia &&
                        trivia.Token.Kind() == SyntaxKind.CloseBraceToken &&
                        trivia.Token.Parent is BlockSyntax blockSyntax &&
                        blockSyntax.Statements.Count == 0
                    ) {
                        docs.Add(Doc.HardLineIfNoPreviousLine);
                    }
                    docs.Add(
                        Doc.HardLineIfNoPreviousLine,
                        Doc.Trim,
                        trivia.ToString(),
                        Doc.HardLine
                    );
                }
                else if (IsRegion(kind))
                {
                    var triviaText = trivia.ToString();
                    if (
                        x > 0 &&
                        leadingTrivia[x - 1].Kind() ==
                        SyntaxKind.WhitespaceTrivia
                    ) {
                        triviaText = leadingTrivia[x - 1] + triviaText;
                    }

                    docs.Add(
                        Doc.HardLineIfNoPreviousLine,
                        Doc.Trim,
                        triviaText,
                        Doc.HardLine
                    );
                }
            }

            if (skipLastHardline && docs.Any() && docs.Last() is HardLine)
            {
                docs.RemoveAt(docs.Count - 1);
            }

            return docs.Count > 0 ? Doc.Concat(docs) : Doc.Null;
        }

        private static bool IsSingleLineComment(SyntaxKind kind) =>
            kind == SyntaxKind.SingleLineDocumentationCommentTrivia ||
            kind == SyntaxKind.SingleLineCommentTrivia;

        private static bool IsMultiLineComment(SyntaxKind kind) =>
            kind == SyntaxKind.MultiLineCommentTrivia ||
            kind == SyntaxKind.MultiLineDocumentationCommentTrivia;

        private static bool IsDirective(SyntaxKind kind) =>
            kind == SyntaxKind.IfDirectiveTrivia ||
            kind == SyntaxKind.ElseDirectiveTrivia ||
            kind == SyntaxKind.ElifDirectiveTrivia ||
            kind == SyntaxKind.EndIfDirectiveTrivia ||
            kind == SyntaxKind.LineDirectiveTrivia ||
            kind == SyntaxKind.ErrorDirectiveTrivia ||
            kind == SyntaxKind.WarningDirectiveTrivia ||
            kind == SyntaxKind.PragmaWarningDirectiveTrivia ||
            kind == SyntaxKind.PragmaChecksumDirectiveTrivia ||
            kind == SyntaxKind.DefineDirectiveTrivia ||
            kind == SyntaxKind.UndefDirectiveTrivia ||
            kind == SyntaxKind.NullableDirectiveTrivia;

        private static bool IsRegion(SyntaxKind kind) =>
            kind == SyntaxKind.RegionDirectiveTrivia ||
            kind == SyntaxKind.EndRegionDirectiveTrivia;

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
                        Doc.TrailingComment(
                            trivia.ToString(),
                            CommentType.SingleLine
                        )
                    );
                }
                else if (trivia.Kind() == SyntaxKind.MultiLineCommentTrivia)
                {
                    docs.Add(
                        " ",
                        Doc.TrailingComment(
                            trivia.ToString(),
                            CommentType.MultiLine
                        )
                    );
                }
            }

            return docs.Count > 0 ? Doc.Concat(docs) : Doc.Null;
        }
    }
}
