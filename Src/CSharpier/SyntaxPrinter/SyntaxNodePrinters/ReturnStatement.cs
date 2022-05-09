namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ReturnStatement
{
    public static Doc Print(ReturnStatementSyntax node, FormattingContext context)
    {
        return Doc.Group(
            ExtraNewLines.Print(node),
            Token.PrintWithSuffix(node.ReturnKeyword, node.Expression != null ? " " : Doc.Null, context),
            node.Expression != null
              ? node.Expression is BinaryExpressionSyntax
                  ? Doc.Indent(Node.Print(node.Expression, context))
                  : Node.Print(node.Expression, context)
              : Doc.Null,
            Token.Print(node.SemicolonToken, context)
        );
    }
}
