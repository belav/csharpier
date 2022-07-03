namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CatchClause
{
    public static Doc Print(CatchClauseSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.Print(node.CatchKeyword, context),
            Doc.Group(
                node.Declaration != null
                    ? Doc.Concat(
                        " ",
                        Token.Print(node.Declaration.OpenParenToken, context),
                        Node.Print(node.Declaration.Type, context),
                        node.Declaration.Identifier.RawSyntaxKind() != SyntaxKind.None
                            ? " "
                            : Doc.Null,
                        Token.Print(node.Declaration.Identifier, context),
                        Token.Print(node.Declaration.CloseParenToken, context)
                    )
                    : Doc.Null,
                node.Filter != null
                    ? Doc.Indent(
                        Doc.Line,
                        Token.PrintWithSuffix(node.Filter.WhenKeyword, " ", context),
                        Token.Print(node.Filter.OpenParenToken, context),
                        Doc.Group(
                            Doc.Indent(Node.Print(node.Filter.FilterExpression, context)),
                            Doc.SoftLine
                        ),
                        Token.Print(node.Filter.CloseParenToken, context)
                    )
                    : Doc.Null
            ),
            Block.Print(node.Block, context)
        );
    }
}
