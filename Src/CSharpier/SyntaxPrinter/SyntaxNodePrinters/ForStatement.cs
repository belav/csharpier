namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ForStatement
{
    public static Doc Print(ForStatementSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>
        {
            ExtraNewLines.Print(node),
            Doc.Group(
                Token.Print(node.ForKeyword, context),
                " ",
                Token.Print(node.OpenParenToken, context),
                Doc.Group(
                    Doc.Indent(
                        Doc.SoftLine,
                        Doc.Group(
                            node.Declaration != null
                                ? VariableDeclaration.Print(node.Declaration, context)
                                : Doc.Null,
                            SeparatedSyntaxList.Print(node.Initializers, Node.Print, " ", context),
                            Token.Print(node.FirstSemicolonToken, context)
                        ),
                        node.Condition != null
                            ? Doc.Concat(Doc.Line, Node.Print(node.Condition, context))
                            : Doc.Line,
                        Token.Print(node.SecondSemicolonToken, context),
                        Doc.Line,
                        Doc.Group(
                            Doc.Indent(
                                SeparatedSyntaxList.Print(
                                    node.Incrementors,
                                    Node.Print,
                                    Doc.Line,
                                    context
                                )
                            )
                        )
                    ),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken, context)
            ),
            node.Statement switch
            {
                ForStatementSyntax => Doc.Group(Doc.HardLine, Node.Print(node.Statement, context)),
                _ => OptionalBraces.Print(node.Statement, context),
            },
        };

        return Doc.Concat(docs);
    }
}
