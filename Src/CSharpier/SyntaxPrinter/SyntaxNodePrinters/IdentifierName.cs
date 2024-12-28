namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class IdentifierName
{
    public static Doc Print(IdentifierNameSyntax node, PrintingContext context)
    {
        return Token.Print(node.Identifier, context);
    }
}
