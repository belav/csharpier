using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class PostfixUnaryExpression
    {
        public static Doc Print(PostfixUnaryExpressionSyntax node)
        {
            return Doc.Concat(Node.Print(node.Operand), Token.Print(node.OperatorToken));
        }
    }
}
