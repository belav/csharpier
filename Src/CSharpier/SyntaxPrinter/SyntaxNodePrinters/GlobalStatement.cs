namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class GlobalStatement
{
    public static Doc Print(GlobalStatementSyntax node)
    {
        return Doc.Concat(
            AttributeLists.Print(node, node.AttributeLists),
            Modifiers.Print(node.Modifiers),
            Node.Print(node.Statement)
        );
    }
}
