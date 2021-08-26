using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ThrowStatement
    {
        public static Doc Print(ThrowStatementSyntax node)
        {
            Doc expression =
                node.Expression != null
                    ? Doc.Concat(" ", Node.Print(node.Expression))
                    : string.Empty;
            return Doc.Concat(
                ExtraNewLines.Print(node),
                Token.Print(node.ThrowKeyword),
                expression,
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
