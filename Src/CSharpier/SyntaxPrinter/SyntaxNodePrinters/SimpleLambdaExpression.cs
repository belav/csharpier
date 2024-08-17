namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SimpleLambdaExpression
{
    public static Doc Print(SimpleLambdaExpressionSyntax node, FormattingContext context)
    {
        return Doc.Group(PrintHead(node, context), PrintBody(node, context));
    }

    public static Doc PrintHead(SimpleLambdaExpressionSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Modifiers.PrintSorted(node.Modifiers, context),
            Node.Print(node.Parameter, context),
            " ",
            Token.Print(node.ArrowToken, context)
        );
    }

    public static Doc PrintBody(SimpleLambdaExpressionSyntax node, FormattingContext context)
    {
        return node.Body switch
        {
            BlockSyntax blockSyntax => Block.Print(blockSyntax, context),
            ObjectCreationExpressionSyntax or AnonymousObjectCreationExpressionSyntax => Doc.Group(
                " ",
                Node.Print(node.Body, context)
            ),
            _ => Doc.Indent(Doc.Line, Node.Print(node.Body, context)),
        };
    }
}
