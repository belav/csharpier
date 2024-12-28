namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SelectClause
{
    public static Doc Print(SelectClauseSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.SelectKeyword, " ", context),
            Node.Print(node.Expression, context)
        );
    }
}
