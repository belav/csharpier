namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ExplicitInterfaceSpecifier
{
    public static Doc Print(ExplicitInterfaceSpecifierSyntax node, PrintingContext context)
    {
        return Doc.Concat(Node.Print(node.Name, context), Token.Print(node.DotToken, context));
    }
}
