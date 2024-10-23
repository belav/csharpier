namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class GlobalStatement
{
    public static Doc Print(GlobalStatementSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            AttributeLists.Print(node, node.AttributeLists, context),
            Modifiers.Print(node.Modifiers, context),
            Node.Print(node.Statement, context)
        );
    }
}
