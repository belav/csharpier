namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class FromClause
{
    public static Doc Print(FromClauseSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.PrintWithSuffix(node.FromKeyword, " ", context),
            node.Type != null ? Doc.Concat(Node.Print(node.Type, context), " ") : Doc.Null,
            Token.PrintWithSuffix(node.Identifier, " ", context),
            Token.PrintWithSuffix(node.InKeyword, " ", context),
            Node.Print(node.Expression, context)
        );
    }
}
