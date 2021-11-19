namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DoStatement
{
    public static Doc Print(DoStatementSyntax node)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(
                node.DoKeyword,
                node.Statement is not BlockSyntax ? " " : Doc.Null
            ),
            Node.Print(node.Statement),
            node.Statement is BlockSyntax ? " " : Doc.HardLine,
            Token.PrintWithSuffix(node.WhileKeyword, " "),
            Token.Print(node.OpenParenToken),
            Doc.Group(Doc.Indent(Doc.SoftLine, Node.Print(node.Condition)), Doc.SoftLine),
            Token.Print(node.CloseParenToken),
            Token.Print(node.SemicolonToken)
        );
    }
}
