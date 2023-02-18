namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class InitializerExpression
{
    public static Doc Print(InitializerExpressionSyntax node, FormattingContext context)
    {
        Doc separator = node.Parent
            is AssignmentExpressionSyntax
                or EqualsValueClauseSyntax { Parent: not PropertyDeclarationSyntax }
            ? Doc.Line
            : Doc.Null;

        var alwaysBreak =
            (
                node.Expressions.Count >= 3
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

        var result = Doc.Concat(
            separator,
            Token.Print(node.OpenBraceToken, context),
            Doc.Indent(
                alwaysBreak ? Doc.HardLine : Doc.Line,
                SeparatedSyntaxList.Print(
                    node.Expressions,
                    Node.Print,
                    alwaysBreak ? Doc.HardLine : Doc.Line,
                    context
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
