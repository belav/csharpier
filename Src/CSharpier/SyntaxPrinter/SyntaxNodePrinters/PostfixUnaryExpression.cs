using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class PostfixUnaryExpression
    {
        public static Doc Print(PostfixUnaryExpressionSyntax node)
        {
            return Doc.Concat(Node.Print(node.Operand), Token.Print(node.OperatorToken));
        }
    }
}
