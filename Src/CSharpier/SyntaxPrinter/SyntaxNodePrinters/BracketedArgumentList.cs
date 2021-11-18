namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BracketedArgumentList
{
    public static Doc Print(BracketedArgumentListSyntax node)
    {
        return Doc.Group(
            Token.Print(node.OpenBracketToken),
            Doc.Indent(
                Doc.SoftLine,
                SeparatedSyntaxList.Print(node.Arguments, Node.Print, Doc.Line)
            ),
            Doc.SoftLine,
            Token.Print(node.CloseBracketToken)
        );
    }
}
