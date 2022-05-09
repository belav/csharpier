namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class DoStatement
{
    public static Doc Print(DoStatementSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(
                node.DoKeyword,
                node.Statement is not BlockSyntax ? " " : Doc.Null,
                context
            ),
            Node.Print(node.Statement, context),
            node.Statement is BlockSyntax ? " " : Doc.HardLine,
            Token.PrintWithSuffix(node.WhileKeyword, " ", context),
            Token.Print(node.OpenParenToken, context),
            Doc.Group(Doc.Indent(Doc.SoftLine, Node.Print(node.Condition, context)), Doc.SoftLine),
            Token.Print(node.CloseParenToken, context),
            Token.Print(node.SemicolonToken, context)
        );
    }
}
