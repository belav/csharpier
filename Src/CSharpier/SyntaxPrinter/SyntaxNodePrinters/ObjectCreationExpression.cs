namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ObjectCreationExpression
{
    public static Doc Print(ObjectCreationExpressionSyntax node)
    {
        return BreakParentIfNested(
            node,
            Doc.Group(
                Token.PrintWithSuffix(node.NewKeyword, " "),
                Node.Print(node.Type),
                node.ArgumentList != null
                  ? Doc.Group(
                        ArgumentListLike.Print(
                            node.ArgumentList.OpenParenToken,
                            node.ArgumentList.Arguments,
                            node.ArgumentList.CloseParenToken
                        )
                    )
                  : Doc.Null,
                node.Initializer != null
                  ? Doc.Concat(Doc.Line, InitializerExpression.Print(node.Initializer))
                  : Doc.Null
            )
        );
    }

    public static Doc BreakParentIfNested(BaseObjectCreationExpressionSyntax node, Doc doc)
    {
        return
            (
                node.Parent?.Parent is InitializerExpressionSyntax
                || node.Parent is InitializerExpressionSyntax
            )
            && node.Initializer != null
          ? Doc.Concat(doc, Doc.BreakParent)
          : doc;
    }
}
