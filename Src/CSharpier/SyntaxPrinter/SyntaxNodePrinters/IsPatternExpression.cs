namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class IsPatternExpression
{
    public static Doc Print(IsPatternExpressionSyntax node, PrintingContext context)
    {
        if (
            node.Parent
            is not (IfStatementSyntax or ParenthesizedExpressionSyntax)
                or ParenthesizedExpressionSyntax { Parent: EqualsValueClauseSyntax }
        )
        {
            return Doc.Group(
                Node.Print(node.Expression, context),
                Doc.Indent(
                    Doc.Line,
                    Token.Print(node.IsKeyword, context),
                    " ",
                    Node.Print(node.Pattern, context)
                )
            );
        }

        if (node.Pattern is not RecursivePatternSyntax recursivePattern)
        {
            return Doc.Group(
                Node.Print(node.Expression, context),
                Doc.Line,
                Token.Print(node.IsKeyword, context),
                " ",
                Node.Print(node.Pattern, context)
            );
        }

        if (recursivePattern.Type is null)
        {
            return Doc.Group(
                Node.Print(node.Expression, context),
                " ",
                Token.Print(node.IsKeyword, context),
                Doc.Line,
                RecursivePattern.Print(recursivePattern, context)
            );
        }

        return Doc.Group(
            Doc.Group(
                Node.Print(node.Expression, context),
                Doc.Line,
                Token.Print(node.IsKeyword, context),
                " ",
                Node.Print(recursivePattern.Type, context)
            ),
            RecursivePattern.PrintWithOutType(recursivePattern, context)
        );
    }
}
