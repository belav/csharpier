namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class NullableType
{
    public static Doc Print(NullableTypeSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Node.Print(node.ElementType, context),
            Token.Print(node.QuestionToken, context)
        );
    }
}
