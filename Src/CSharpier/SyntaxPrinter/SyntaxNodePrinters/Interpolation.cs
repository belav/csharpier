namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class Interpolation
{
    public static Doc Print(InterpolationSyntax node, FormattingContext context)
    {
        var docs = new List<Doc>
        {
            Token.Print(node.OpenBraceToken, context),
            Node.Print(node.Expression, context),
        };
        if (node.AlignmentClause != null)
        {
            docs.Add(
                Token.PrintWithSuffix(node.AlignmentClause.CommaToken, " ", context),
                Node.Print(node.AlignmentClause.Value, context)
            );
        }
        if (node.FormatClause != null)
        {
            docs.Add(
                Token.Print(node.FormatClause.ColonToken, context),
                Token.Print(node.FormatClause.FormatStringToken, context)
            );
        }

        docs.Add(Token.Print(node.CloseBraceToken, context));
        return Doc.Concat(docs);
    }
}
