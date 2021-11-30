namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitObjectCreationExpression
{
    public static Doc Print(ImplicitObjectCreationExpressionSyntax node)
    {
        return ObjectCreationExpression.BreakParentIfNested(
            node,
            Doc.Group(
                Token.Print(node.NewKeyword),
                ArgumentList.Print(node.ArgumentList),
                node.Initializer != null
                  ? Doc.Concat(Doc.Line, InitializerExpression.Print(node.Initializer))
                  : Doc.Null
            )
        );
    }
}
