namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class VarPattern
{
    public static Doc Print(VarPatternSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.VarKeyword, " ", context),
            Node.Print(node.Designation, context)
        );
    }
}
