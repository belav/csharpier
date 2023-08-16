namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitObjectCreationExpression
{
    public static Doc Print(ImplicitObjectCreationExpressionSyntax node, FormattingContext context)
    {
        return ObjectCreationExpression.BreakParentIfNested(
            node,
            Doc.Group(
                Token.Print(node.NewKeyword, context),
                ArgumentList.Print(node.ArgumentList, context),
                node.Initializer != null
                    ? Doc.Concat(" ", InitializerExpression.Print(node.Initializer, context))
                    : Doc.Null
            )
        );
    }
}
