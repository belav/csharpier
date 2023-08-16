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
                        Doc.Indent(
                            Doc.SoftLine,
                            SeparatedSyntaxList.Print(
                                node.Parameters,
                                Parameter.Print,
                                Doc.IfBreak(Doc.Line, " "),
                                context
                            )
                        )
                    )
                )
                : Doc.Null,
            Token.Print(node.CloseParenToken, context)
        );
    }
}
