namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TupleType
{
    public static Doc Print(TupleTypeSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.Print(node.OpenParenToken, context),
            SeparatedSyntaxList.Print(node.Elements, Node.Print, " ", context),
            Token.Print(node.CloseParenToken, context)
        );
    }
}
