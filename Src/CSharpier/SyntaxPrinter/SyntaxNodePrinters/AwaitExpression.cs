using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class AwaitExpression
    {
        public static Doc Print(AwaitExpressionSyntax node)
        {
            return Doc.Concat(Token.Print(node.AwaitKeyword, " "), Node.Print(node.Expression));
        }
    }
}
