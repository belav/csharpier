using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitObjectCreationExpression
{
    public static Doc Print(ImplicitObjectCreationExpressionSyntax node)
    {
        return Doc.Group(
            Token.Print(node.NewKeyword),
            ArgumentList.Print(node.ArgumentList),
            node.Initializer != null
              ? Doc.Concat(Doc.Line, InitializerExpression.Print(node.Initializer))
              : Doc.Null
        );
    }
}
