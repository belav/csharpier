namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class FixedStatement
{
    public static Doc Print(FixedStatementSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            ExtraNewLines.Print(node),
            Doc.Group(
                Token.Print(node.FixedKeyword, context),
                " ",
                Token.Print(node.OpenParenToken, context),
                Doc.Group(
                    Doc.Indent(Doc.SoftLine, Node.Print(node.Declaration, context)),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken, context),
                Doc.IfBreak(Doc.Null, Doc.SoftLine)
            ),
            node.Statement is BlockSyntax blockSyntax
              ? Block.Print(blockSyntax, context)
              : Doc.IndentIf(
                    node.Statement is not FixedStatementSyntax,
                    Doc.Concat(Doc.HardLine, Node.Print(node.Statement, context))
                )
        );
    }
}
