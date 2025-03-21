namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class Interpolation
{
    public static Doc Print(InterpolationSyntax node, PrintingContext context)
    {
        var docs = new ValueListBuilder<Doc>([null, null, null, null, null, null, null]);
        docs.Append(Token.Print(node.OpenBraceToken, context));
        docs.Append(Node.Print(node.Expression, context));

        if (node.AlignmentClause != null)
        {
            docs.Append(
                Token.PrintWithSuffix(node.AlignmentClause.CommaToken, " ", context),
                Node.Print(node.AlignmentClause.Value, context)
            );
        }
        if (node.FormatClause != null)
        {
            docs.Append(
                Token.Print(node.FormatClause.ColonToken, context),
                Token.Print(node.FormatClause.FormatStringToken, context)
            );
        }

        docs.Append(Token.Print(node.CloseBraceToken, context));
        return Doc.Concat(ref docs);
    }
}
