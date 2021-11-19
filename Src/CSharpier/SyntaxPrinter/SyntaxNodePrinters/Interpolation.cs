using CSharpier.Utilities;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class Interpolation
{
    public static Doc Print(InterpolationSyntax node)
    {
        var docs = new List<Doc> { Token.Print(node.OpenBraceToken), Node.Print(node.Expression) };
        if (node.AlignmentClause != null)
        {
            docs.Add(
                Token.PrintWithSuffix(node.AlignmentClause.CommaToken, " "),
                Node.Print(node.AlignmentClause.Value)
            );
        }
        if (node.FormatClause != null)
        {
            docs.Add(
                Token.Print(node.FormatClause.ColonToken),
                Token.Print(node.FormatClause.FormatStringToken)
            );
        }

        docs.Add(Token.Print(node.CloseBraceToken));
        return Doc.Concat(docs);
    }
}
