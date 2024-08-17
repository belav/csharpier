namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class WhileStatement
{
    public static Doc Print(WhileStatementSyntax node, FormattingContext context)
    {
        var result = Doc.Concat(
            ExtraNewLines.Print(node),
            Doc.Group(
                Token.Print(node.WhileKeyword, context),
                " ",
                Token.Print(node.OpenParenToken, context),
                Doc.Group(
                    Doc.Indent(Doc.SoftLine, Node.Print(node.Condition, context)),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken, context)
            ),
            node.Statement switch
            {
                WhileStatementSyntax => Doc.Group(
                    Doc.HardLine,
                    Node.Print(node.Statement, context)
                ),
                _ => OptionalBraces.Print(node.Statement, context),
            }
        );

        return result;
    }
}
