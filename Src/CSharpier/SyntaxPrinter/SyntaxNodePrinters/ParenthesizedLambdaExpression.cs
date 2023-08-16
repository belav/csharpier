namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ParenthesizedLambdaExpression
{
    public static Doc Print(ParenthesizedLambdaExpressionSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>
        {
            AttributeLists.Print(node, node.AttributeLists, context),
            Modifiers.Print(node.Modifiers, context),
            node.ReturnType != null
                ? Doc.Concat(Node.Print(node.ReturnType, context), " ")
                : Doc.Null,
            ParameterList.Print(node.ParameterList, context),
            " ",
            Token.Print(node.ArrowToken, context)
        };
        if (node.ExpressionBody != null)
        {
            docs.Add(Doc.Group(Doc.Indent(Doc.Line, Node.Print(node.ExpressionBody, context))));
        }
        else if (node.Block != null)
        {
            docs.Add(Doc.IfBreak(Doc.Line, " "));
            docs.Add(Block.Print(node.Block, context));
        }

        return Doc.Concat(docs);
    }
}
