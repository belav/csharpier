namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ParameterList
{
    public static Doc Print(ParameterListSyntax node, FormattingContext context)
    {
        return Doc.Group(
            Token.Print(node.OpenParenToken, context),
            node.Parameters.Count > 0
                ? Doc.Concat(
                    Doc.Indent(
                        Doc.SoftLine,
                        SeparatedSyntaxList.Print(
                            node.Parameters,
                            Parameter.Print,
                            Doc.Line,
                            context
                        )
                    ),
                    Doc.SoftLine
                )
                : Doc.Null,
            Token.Print(node.CloseParenToken, context)
        );
    }
}
