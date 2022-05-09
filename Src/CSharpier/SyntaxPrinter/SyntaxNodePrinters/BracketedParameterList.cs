namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BracketedParameterList
{
    public static Doc Print(BracketedParameterListSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.Print(node.OpenBracketToken, context),
            SeparatedSyntaxList.Print(node.Parameters, Parameter.Print, " ", context),
            Token.Print(node.CloseBracketToken, context)
        );
    }
}
