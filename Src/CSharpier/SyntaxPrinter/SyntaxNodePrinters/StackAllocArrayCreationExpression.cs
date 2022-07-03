namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class StackAllocArrayCreationExpression
{
    public static Doc Print(StackAllocArrayCreationExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.StackAllocKeyword, " ", context),
            Node.Print(node.Type, context),
            node.Initializer != null
                ? Doc.Concat(" ", InitializerExpression.Print(node.Initializer, context))
                : string.Empty
        );
    }
}
