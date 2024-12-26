namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ArgumentList
{
    public static Doc Print(ArgumentListSyntax node, PrintingContext context)
    {
        return Doc.Group(
            Doc.IndentIf(
                // indent if this is the first argumentList in a method chain
                node.Parent
                    is InvocationExpressionSyntax
                    {
                        Expression: IdentifierNameSyntax
                            or GenericNameSyntax
                            or MemberAccessExpressionSyntax
                            {
                                Expression: ThisExpressionSyntax
                                    or PredefinedTypeSyntax
                                    or IdentifierNameSyntax
                                    {
                                        Identifier: { Text: { Length: <= 4 } }
                                    }
                            },
                        Parent: { Parent: InvocationExpressionSyntax }
                            or PostfixUnaryExpressionSyntax
                            {
                                Parent: { Parent: InvocationExpressionSyntax }
                            }
                    },
                ArgumentListLike.Print(
                    node.OpenParenToken,
                    node.Arguments,
                    node.CloseParenToken,
                    context
                )
            )
        );
    }
}
