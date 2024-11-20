namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BaseExpressionColon
{
    public static Doc Print(BaseExpressionColonSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Expression, context),
            Token.PrintWithSuffix(
                node.ColonToken,
                node.Parent is SubpatternSyntax { Pattern: RecursivePatternSyntax { Type: null } }
                    ? Doc.Line
                    : " ",
                context
            )
        );
    }
}
