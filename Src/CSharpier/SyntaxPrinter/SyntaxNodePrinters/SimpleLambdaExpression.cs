namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SimpleLambdaExpression
{
    public static Doc Print(SimpleLambdaExpressionSyntax node, FormattingContext context)
    {
        return Doc.Group(
            Modifiers.Print(node.Modifiers, context),
            Node.Print(node.Parameter, context),
            " ",
            Token.Print(node.ArrowToken, context),
            node.Body is BlockSyntax blockSyntax
              ? Block.Print(blockSyntax, context)
              : Doc.Indent(Doc.Line, Node.Print(node.Body, context))
        );
    }
}
