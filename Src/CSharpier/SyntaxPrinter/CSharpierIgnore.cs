namespace CSharpier.SyntaxPrinter;

internal static class CSharpierIgnore
{
    public static bool IsNodeIgnored(SyntaxNode syntaxNode)
    {
        if (
            syntaxNode.Parent
            is not (
                BaseTypeDeclarationSyntax
                or BlockSyntax
                or CompilationUnitSyntax
                or NamespaceDeclarationSyntax
            )
        )
        {
            return false;
        }

        return (Token.HasLeadingComment(syntaxNode, "// csharpier-ignore"));
    }

    public static List<Doc> PrintNodesRespectingRangeIgnore<T>(
        SyntaxList<T> list,
        FormattingContext context
    ) where T : SyntaxNode
    {
        var statements = new List<Doc>();
        var printUnformatted = false;

        foreach (var node in list)
        {
            if (Token.HasLeadingComment(node, "// csharpier-ignore-end"))
            {
                printUnformatted = false;
            }
            else if (Token.HasLeadingComment(node, "// csharpier-ignore-start"))
            {
                printUnformatted = true;
            }

            statements.Add(
                printUnformatted ? PrintWithoutFormatting(node) : Node.Print(node, context)
            );
        }

        return statements;
    }

    public static Doc PrintWithoutFormatting(SyntaxNode syntaxNode)
    {
        return syntaxNode.GetText().ToString().Trim();
    }
}
