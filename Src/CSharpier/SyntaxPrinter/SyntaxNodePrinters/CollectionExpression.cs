namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CollectionExpression
{
    public static Doc Print(CollectionExpressionSyntax node, FormattingContext context)
    {
        Doc separator = node.Parent
            is ArgumentSyntax { NameColon: null }
                or ArrowExpressionClauseSyntax
                or ExpressionElementSyntax
                or AssignmentExpressionSyntax
                {
                    Parent: not (
                        ObjectCreationExpressionSyntax
                        or InitializerExpressionSyntax
                        or ExpressionStatementSyntax
                    )
                }
                or EqualsValueClauseSyntax
                {
                    Parent: not (PropertyDeclarationSyntax or VariableDeclaratorSyntax)
                }
            ? Doc.Null
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
            node.CloseBracketToken.LeadingTrivia.Any(o => o.IsComment())
                ? Doc.Concat(
                    Doc.Indent(Token.PrintLeadingTrivia(node.CloseBracketToken, context)),
                    Doc.HardLine
                )
                : Doc.Null,
            Token.PrintWithoutLeadingTrivia(node.CloseBracketToken, context)
        );
        return Doc.Group(result);
    }
}
