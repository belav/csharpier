using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitObjectCreationExpression
{
    public static Doc Print(ImplicitObjectCreationExpressionSyntax node, PrintingContext context)
    {
        return ObjectCreationExpression.BreakParentIfNested(
            node,
            Doc.Group(
                Token.Print(node.NewKeyword, context),
                ArgumentList.Print(node.ArgumentList, context),
                node.Initializer != null
                    ? Doc.Concat(Doc.Line, InitializerExpression.Print(node.Initializer, context))
                    : Doc.Null
            )
        );
    }
}
