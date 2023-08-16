namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class InitializerExpression
{
    public static Doc Print(InitializerExpressionSyntax node, FormattingContext context)
    {
        var alwaysBreak =
            (
                node.Expressions.Count >= 5
                && (
                    node.Kind() is SyntaxKind.ObjectInitializerExpression
                    || node.Kind() is SyntaxKind.CollectionInitializerExpression
                        && node.Expressions.FirstOrDefault()?.Kind()
                            is SyntaxKind.ComplexElementInitializerExpression
                )
            )
            || (
                node.Kind() is SyntaxKind.ArrayInitializerExpression
                && node.Expressions.FirstOrDefault()?.Kind()
                    is SyntaxKind.ArrayInitializerExpression
            );
        var addTrailingSeparator = node.Kind() is not SyntaxKind.ComplexElementInitializerExpression;

        var result = Doc.Concat(
            Token.Print(node.OpenBraceToken, context),
            Doc.Indent(
                alwaysBreak ? Doc.HardLine : Doc.Line,
                SeparatedSyntaxList.Print(
                    node.Expressions,
                    Node.Print,
                    alwaysBreak ? Doc.HardLine : Doc.Line,
                    context,
                    addTrailingSeparator,
                    separator: ","
                )
            ),
            node.Expressions.Any()
                ? alwaysBreak
                    ? Doc.HardLine
                    : Doc.Line
                : Doc.Null,
            Token.Print(node.CloseBraceToken, context)
        );
        return
            node.Parent
                is not (
                    ObjectCreationExpressionSyntax
                    or ArrayCreationExpressionSyntax
                    or ImplicitArrayCreationExpressionSyntax
                    or ImplicitObjectCreationExpressionSyntax
                )
            && node.Kind() is not SyntaxKind.WithInitializerExpression
            ? Doc.Group(result)
            : result;
    }
}
