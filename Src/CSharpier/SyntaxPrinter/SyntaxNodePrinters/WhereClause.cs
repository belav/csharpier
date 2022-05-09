namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class WhereClause
{
    public static Doc Print(WhereClauseSyntax node, FormattingContext context)
    {
        return Doc.Group(
            Token.Print(node.WhereKeyword, context),
            Doc.Indent(Doc.Line, Node.Print(node.Condition, context))
        );
    }
}
