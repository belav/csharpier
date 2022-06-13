namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ElseClause
{
    public static Doc Print(ElseClauseSyntax node, FormattingContext context)
    {
        return Doc.Concat(
            Token.Print(node.ElseKeyword, context),
            node.Statement is IfStatementSyntax ifStatementSyntax
                ? Doc.Concat(" ", IfStatement.Print(ifStatementSyntax, context))
                : OptionalBraces.Print(node.Statement, context)
        );
    }
}
