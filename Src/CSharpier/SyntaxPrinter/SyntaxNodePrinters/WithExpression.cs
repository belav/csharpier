using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class WithExpression
    {
        public static Doc Print(WithExpressionSyntax node)
        {
            return Doc.Concat(
                Node.Print(node.Expression),
                " ",
                Token.PrintWithSuffix(node.WithKeyword, Doc.Line),
                Node.Print(node.Initializer)
            );
        }
    }
}
