using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ImplicitObjectCreationExpression
    {
        public static Doc Print(ImplicitObjectCreationExpressionSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.NewKeyword),
                ArgumentList.Print(node.ArgumentList),
                node.Initializer != null ? Node.Print(node.Initializer) : Doc.Null
            );
        }
    }
}
