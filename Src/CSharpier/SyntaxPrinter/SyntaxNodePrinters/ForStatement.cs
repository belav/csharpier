namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ForStatement
{
    public static Doc Print(ForStatementSyntax node)
    {
        var docs = new List<Doc>
        {
            ExtraNewLines.Print(node),
            Doc.Group(
                Token.Print(node.ForKeyword),
                " ",
                Token.Print(node.OpenParenToken),
                Doc.Group(
                    Doc.Indent(
                        Doc.SoftLine,
                        Doc.Group(
                            node.Declaration != null
                              ? VariableDeclaration.Print(node.Declaration)
                              : Doc.Null,
                            SeparatedSyntaxList.Print(node.Initializers, Node.Print, " "),
                            Token.Print(node.FirstSemicolonToken)
                        ),
                        node.Condition != null
                          ? Doc.Concat(Doc.Line, Node.Print(node.Condition))
                          : Doc.Line,
                        Token.Print(node.SecondSemicolonToken),
                        Doc.Line,
                        Doc.Group(
                            Doc.Indent(
                                SeparatedSyntaxList.Print(node.Incrementors, Node.Print, Doc.Line)
                            )
                        )
                    ),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken)
            ),
            OptionalBraces.Print(node.Statement)
        };

        return Doc.Concat(docs);
    }
}
