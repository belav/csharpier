namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CommonForEachStatement
{
    public static Doc Print(CommonForEachStatementSyntax node)
    {
        var variable = node switch
        {
            ForEachStatementSyntax forEach
              => Doc.Concat(Node.Print(forEach.Type), " ", Token.Print(forEach.Identifier)),
            ForEachVariableStatementSyntax forEachVariable => Node.Print(forEachVariable.Variable),
            _ => Doc.Null
        };

        var docs = Doc.Concat(
            ExtraNewLines.Print(node),
            Doc.Group(
                Token.Print(node.AwaitKeyword),
                node.AwaitKeyword.RawSyntaxKind() is not SyntaxKind.None ? " " : Doc.Null,
                Token.Print(node.ForEachKeyword),
                " ",
                Token.Print(node.OpenParenToken),
                Doc.Group(
                    Doc.Indent(
                        Doc.SoftLine,
                        variable,
                        " ",
                        Token.Print(node.InKeyword),
                        " ",
                        Node.Print(node.Expression)
                    ),
                    Doc.SoftLine
                ),
                Token.Print(node.CloseParenToken)
            ),
            OptionalBraces.Print(node.Statement)
        );

        return docs;
    }
}
