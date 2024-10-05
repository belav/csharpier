namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypeArgumentList
{
    public static Doc Print(TypeArgumentListSyntax node, PrintingContext context)
    {
        Doc separator =
            node.Arguments.FirstOrDefault() is not OmittedTypeArgumentSyntax
            && (node.Arguments.Count > 1 || node.Arguments.Any(o => o is GenericNameSyntax))
                ? Doc.SoftLine
                : Doc.Null;

        return Doc.Concat(
            Token.Print(node.LessThanToken, context),
            Doc.IndentIf(
                separator != Doc.Null,
                Doc.Concat(
                    separator,
                    SeparatedSyntaxList.Print(
                        node.Arguments,
                        Node.Print,
                        node.Arguments.FirstOrDefault() is OmittedTypeArgumentSyntax
                            ? Doc.Null
                            : Doc.Line,
                        context
                    )
                )
            ),
            separator,
            Token.Print(node.GreaterThanToken, context)
        );
    }
}
