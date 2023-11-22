namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CollectionExpression
{
    public static Doc Print(CollectionExpressionSyntax node, FormattingContext context)
    {
        Doc separator = node.Parent
            is AssignmentExpressionSyntax
                or EqualsValueClauseSyntax
                {
                    Parent: not (PropertyDeclarationSyntax or VariableDeclaratorSyntax)
                }
            ? Doc.Line
            : Doc.IfBreak(Doc.Line, Doc.Null);

        var alwaysBreak =
            node.Elements.FirstOrDefault()
                is ExpressionElementSyntax { Expression: CollectionExpressionSyntax };

        var result = Doc.Concat(
            separator,
            Token.Print(node.OpenBracketToken, context),
            node.Elements.Any()
                ? Doc.Indent(
                    alwaysBreak ? Doc.HardLine : Doc.IfBreak(Doc.Line, Doc.Null),
                    SeparatedSyntaxList.Print(
                        node.Elements,
                        Node.Print,
                        alwaysBreak ? Doc.HardLine : Doc.Line,
                        context
                    )
                )
                : Doc.Null,
            node.Elements.Any()
                ? alwaysBreak
                    ? Doc.HardLine
                    : Doc.IfBreak(Doc.Line, Doc.Null)
                : Doc.Null,
            Token.Print(node.CloseBracketToken, context)
        );
        return Doc.Group(result);
    }
}
