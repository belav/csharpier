namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ImplicitStackAllocArrayCreationExpression
{
    public static Doc Print(
        ImplicitStackAllocArrayCreationExpressionSyntax node,
        FormattingContext context
    )
    {
        return Doc.Concat(
            Token.Print(node.StackAllocKeyword, context),
            Token.Print(node.OpenBracketToken, context),
            Token.PrintWithSuffix(node.CloseBracketToken, " ", context),
            Node.Print(node.Initializer, context)
        );
    }
}
