namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class JoinClause
{
    public static Doc Print(JoinClauseSyntax node)
    {
        return Doc.Group(
            Token.PrintWithSuffix(node.JoinKeyword, " "),
            Token.PrintWithSuffix(node.Identifier, " "),
            Token.PrintWithSuffix(node.InKeyword, " "),
            Node.Print(node.InExpression),
            Doc.Indent(
                Doc.Line,
                Token.PrintWithSuffix(node.OnKeyword, " "),
                Node.Print(node.LeftExpression),
                " ",
                Token.PrintWithSuffix(node.EqualsKeyword, " "),
                Node.Print(node.RightExpression),
                node.Into != null
                  ? Doc.Concat(
                        Doc.Line,
                        Token.PrintWithSuffix(node.Into.IntoKeyword, " "),
                        Token.Print(node.Into.Identifier)
                    )
                  : Doc.Null
            )
        );
    }
}
