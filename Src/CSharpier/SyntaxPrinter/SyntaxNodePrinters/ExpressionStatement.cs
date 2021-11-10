using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class ExpressionStatement
    {
        public static Doc Print(ExpressionStatementSyntax node)
        {
            return Doc.Group(
                ExtraNewLines.Print(node),
                Node.Print(node.Expression),
                Token.Print(node.SemicolonToken)
            );
        }
    }
}
