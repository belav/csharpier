namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class NullableType
{
    public static Doc Print(NullableTypeSyntax node)
    {
        return Doc.Concat(Node.Print(node.ElementType), Token.Print(node.QuestionToken));
    }
}
