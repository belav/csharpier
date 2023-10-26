namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CollectionExpression
{
    public static Doc Print(CollectionExpressionSyntax node, FormattingContext context)
    {
        Doc separator = node.Parent
            is AssignmentExpressionSyntax
                or EqualsValueClauseSyntax { Parent: not PropertyDeclarationSyntax }
            ? Doc.Line
            : Doc.Null;

        var alwaysBreak =
            node.Elements.FirstOrDefault()
                is ExpressionElementSyntax { Expression: CollectionExpressionSyntax };

        var result = Doc.Concat(
            separator,
            Token.Print(node.OpenBracketToken, context),
            Doc.Indent(
                alwaysBreak ? Doc.HardLine : Doc.Line,
                SeparatedSyntaxList.Print(
                    node.Elements,
                    Node.Print,
                    alwaysBreak ? Doc.HardLine : Doc.Line,
                    context
                )
            ),
            node.Elements.Any()
                ? alwaysBreak
                    ? Doc.HardLine
                    : Doc.Line
                : Doc.Null,
            Token.Print(node.CloseBracketToken, context)
        );
        return Doc.Group(result);
    }
}
