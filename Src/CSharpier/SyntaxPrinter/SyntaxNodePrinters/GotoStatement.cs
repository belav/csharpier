using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class GotoStatement
    {
        public static Doc Print(GotoStatementSyntax node)
        {
            Doc expression = node.Expression != null
                ? Doc.Concat(" ", Node.Print(node.Expression))
                : string.Empty;
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.Print(node.GotoKeyword),
                node.CaseOrDefaultKeyword.RawKind != 0 ? " " : Doc.Null,
                Token.Print(node.CaseOrDefaultKeyword),
                expression,
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
