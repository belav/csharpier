namespace CSharpier.SyntaxPrinter;

internal static class OptionalBraces
{
    public static Doc Print(StatementSyntax node)
    {
        return node is BlockSyntax blockSyntax
          ? Block.Print(blockSyntax)
          : Doc.Indent(Doc.HardLine, Node.Print(node));
    }
}
