using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class RefExpression
    {
        public static Doc Print(RefExpressionSyntax node)
        {
            return Doc.Concat(
                Token.PrintWithSuffix(node.RefKeyword, " "),
                Node.Print(node.Expression)
            );
        }
    }
}
