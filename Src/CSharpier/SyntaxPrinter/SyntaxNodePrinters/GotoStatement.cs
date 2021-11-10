using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class GotoStatement
    {
        public static Doc Print(GotoStatementSyntax node)
        {
            Doc expression =
                node.Expression != null
                    ? Doc.Concat(" ", Node.Print(node.Expression))
                    : string.Empty;
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.Print(node.GotoKeyword),
                node.CaseOrDefaultKeyword.Kind() != SyntaxKind.None ? " " : Doc.Null,
                Token.Print(node.CaseOrDefaultKeyword),
                expression,
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
