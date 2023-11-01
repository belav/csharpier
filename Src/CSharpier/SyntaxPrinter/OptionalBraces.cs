namespace CSharpier.SyntaxPrinter;

internal static class OptionalBraces
{
    public static Doc Print(StatementSyntax node, FormattingContext context)
    {
        return node is BlockSyntax blockSyntax
            ? Block.Print(blockSyntax, context)
            : DocUtilities.RemoveInitialDoubleHardLine(
                Doc.Indent(Doc.HardLine, Node.Print(node, context))
            );
    }
}
