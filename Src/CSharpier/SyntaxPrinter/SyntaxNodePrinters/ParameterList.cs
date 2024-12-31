namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ParameterList
{
    public static Doc Print(ParameterListSyntax node, PrintingContext context)
    {
        return Print(node, node.OpenParenToken, node.CloseParenToken, context);
    }

    public static Doc Print(
        BaseParameterListSyntax node,
        SyntaxToken openToken,
        SyntaxToken closeToken,
        PrintingContext context
    )
    {
        return Doc.Group(
            Token.Print(openToken, context),
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
            Token.Print(closeToken, context)
        );
    }
}
