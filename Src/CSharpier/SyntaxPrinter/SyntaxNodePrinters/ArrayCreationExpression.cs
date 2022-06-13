namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ArrayCreationExpression
{
    public static Doc Print(ArrayCreationExpressionSyntax node, FormattingContext context)
    {
        return Doc.Group(
            Token.PrintWithSuffix(node.NewKeyword, " ", context),
            Node.Print(node.Type, context),
            node.Initializer != null
                ? Doc.Concat(Doc.Line, Node.Print(node.Initializer, context))
                : Doc.Null
        );
    }
}
