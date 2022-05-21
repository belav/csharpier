namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BracketedArgumentList
{
    public static Doc Print(BracketedArgumentListSyntax node, FormattingContext context)
    {
        return Doc.Group(
            Token.Print(node.OpenBracketToken, context),
            Doc.Indent(
                Doc.SoftLine,
                SeparatedSyntaxList.Print(node.Arguments, Node.Print, Doc.Line, context)
            ),
            Doc.SoftLine,
            Token.Print(node.CloseBracketToken, context)
        );
    }
}
