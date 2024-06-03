namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BracketedParameterList
{
    public static Doc Print(BracketedParameterListSyntax node, FormattingContext context)
    {
        return ParameterList.Print(node, node.OpenBracketToken, node.CloseBracketToken, context);
    }
}
