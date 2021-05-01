using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ImplicitObjectCreationExpression
    {
        public static Doc Print(ImplicitObjectCreationExpressionSyntax node)
        {
            // TODO 1 more tests for this?
            return Doc.Concat(
                Token.Print(node.NewKeyword),
                ArgumentList.Print(node.ArgumentList),
                Node.Print(node.Initializer)
            );
        }
    }
}
