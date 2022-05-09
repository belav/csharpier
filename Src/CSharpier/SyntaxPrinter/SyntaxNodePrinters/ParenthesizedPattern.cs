namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ParenthesizedPattern
{
    public static Doc Print(ParenthesizedPatternSyntax node, FormattingContext context)
    {
        return Doc.Group(
            Token.Print(node.OpenParenToken, context),
            Doc.Indent(Doc.SoftLine, Node.Print(node.Pattern, context)),
            Doc.SoftLine,
            Token.Print(node.CloseParenToken, context)
        );
    }
}
