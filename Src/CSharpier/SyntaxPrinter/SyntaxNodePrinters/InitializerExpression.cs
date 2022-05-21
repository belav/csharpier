namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class InitializerExpression
{
    public static Doc Print(InitializerExpressionSyntax node, FormattingContext context)
    {
        Doc separator = node.Parent is AssignmentExpressionSyntax or EqualsValueClauseSyntax
            ? Doc.Line
            : Doc.Null;

        var alwaysBreak =
            node.Kind() == SyntaxKind.ObjectInitializerExpression && node.Expressions.Count >= 3;

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
