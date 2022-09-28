namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TupleType
{
    public static Doc Print(TupleTypeSyntax node, FormattingContext context)
    {
        return Doc.Group(
            Token.Print(node.OpenParenToken, context),
            Doc.Indent(
                Doc.SoftLine,
                SeparatedSyntaxList.Print(node.Elements, Node.Print, Doc.Line, context)
            ),
            Doc.SoftLine,
            Token.Print(node.CloseParenToken, context)
        );
    }
}
