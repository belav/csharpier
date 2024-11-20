namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ParenthesizedVariableDesignation
{
    public static Doc Print(ParenthesizedVariableDesignationSyntax node, PrintingContext context)
    {
        return Doc.Group(
            Token.Print(node.OpenParenToken, context),
            Doc.Indent(
                Doc.SoftLine,
                SeparatedSyntaxList.Print(node.Variables, Node.Print, Doc.Line, context)
            ),
            Doc.SoftLine,
            Token.Print(node.CloseParenToken, context)
        );
    }
}
