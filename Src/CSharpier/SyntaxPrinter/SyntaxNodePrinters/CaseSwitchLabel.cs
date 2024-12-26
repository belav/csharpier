namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CaseSwitchLabel
{
    public static Doc Print(CaseSwitchLabelSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.Keyword, " ", context),
            Doc.Group(Node.Print(node.Value, context)),
            Token.Print(node.ColonToken, context)
        );
    }
}
