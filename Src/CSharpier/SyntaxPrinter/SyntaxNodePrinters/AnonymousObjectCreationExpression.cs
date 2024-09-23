namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class AnonymousObjectCreationExpression
{
    public static Doc Print(AnonymousObjectCreationExpressionSyntax node, PrintingContext context)
    {
        var alwaysBreak = node.Initializers.Count >= 3;

        return Doc.Group(
            Token.PrintWithSuffix(node.NewKeyword, Doc.Line, context),
            Token.Print(node.OpenBraceToken, context),
            node.Initializers.Any()
                ? Doc.Indent(
                    alwaysBreak ? Doc.HardLine : Doc.Line,
                    SeparatedSyntaxList.PrintWithTrailingComma(
                        node.Initializers,
                        AnonymousObjectMemberDeclarator.Print,
                        Doc.Line,
                        context,
                        node.CloseBraceToken
                    )
                )
                : Doc.Null,
            Doc.Line,
            Token.Print(node.CloseBraceToken, context)
        );
    }
}
