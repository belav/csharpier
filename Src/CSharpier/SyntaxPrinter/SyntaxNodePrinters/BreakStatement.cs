namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BreakStatement
{
    public static Doc Print(BreakStatementSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.Print(node.BreakKeyword, context),
            Token.Print(node.SemicolonToken, context)
        );
    }
}
