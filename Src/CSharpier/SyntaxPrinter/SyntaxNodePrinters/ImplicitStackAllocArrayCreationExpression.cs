using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ImplicitStackAllocArrayCreationExpression
    {
        public static Doc Print(
            ImplicitStackAllocArrayCreationExpressionSyntax node
        ) {
            return Doc.Concat(
                Token.Print(node.StackAllocKeyword),
                Token.Print(node.OpenBracketToken),
                Token.Print(node.CloseBracketToken, " "),
                Node.Print(node.Initializer)
            );
        }
    }
}
