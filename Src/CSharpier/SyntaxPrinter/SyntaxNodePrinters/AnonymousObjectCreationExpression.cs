namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AnonymousObjectCreationExpression
{
    public static Doc Print(AnonymousObjectCreationExpressionSyntax node)
    {
        return Doc.Group(
            Token.PrintWithSuffix(node.NewKeyword, Doc.Line),
            Token.Print(node.OpenBraceToken),
            node.Initializers.Any()
              ? Doc.Indent(
                    Doc.Line,
                    SeparatedSyntaxList.Print(
                        node.Initializers,
                        AnonymousObjectMemberDeclarator.Print,
                        Doc.Line
                    )
                )
              : Doc.Null,
            Doc.Line,
            Token.Print(node.CloseBraceToken)
        );
    }
}
