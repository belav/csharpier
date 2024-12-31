namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SpreadElement
{
    public static Doc Print(SpreadElementSyntax node, PrintingContext context)
    {
        return Doc.Group(
            Token.Print(node.OperatorToken, context),
            " ",
            Node.Print(node.Expression, context)
        );
    }
}
