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
        InitializerExpressionSyntax? parentInitializerExpressionSyntax = null;
        if (node.Parent is InitializerExpressionSyntax i1)
        {
            parentInitializerExpressionSyntax = i1;
        }
        else if (node.Parent?.Parent is InitializerExpressionSyntax i2)
        {
            parentInitializerExpressionSyntax = i2;
        }

        return
            parentInitializerExpressionSyntax != null
            && node.Initializer != null
            && (
                node.Initializer.Expressions.Count > 1
                || parentInitializerExpressionSyntax.Expressions.Count > 1
            )
          ? Doc.Concat(doc, Doc.BreakParent)
          : doc;
    }
}
