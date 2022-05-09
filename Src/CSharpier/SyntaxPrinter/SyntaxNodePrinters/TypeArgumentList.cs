namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypeArgumentList
{
    public static Doc Print(TypeArgumentListSyntax node, FormattingContext context)
    {
        Doc separator =
            node.Arguments.Count > 1 || node.Arguments.Any(o => o is GenericNameSyntax)
                ? Doc.SoftLine
                : Doc.Null;

        return Doc.Concat(
            Token.Print(node.LessThanToken, context),
            Doc.Indent(
                separator,
                SeparatedSyntaxList.Print(
                    node.Arguments,
                    Node.Print,
                    node.Arguments.FirstOrDefault() is OmittedTypeArgumentSyntax
                      ? Doc.Null
                      : Doc.Line, context
                )
            ),
            separator,
            Token.Print(node.GreaterThanToken, context)
        );
    }
}
