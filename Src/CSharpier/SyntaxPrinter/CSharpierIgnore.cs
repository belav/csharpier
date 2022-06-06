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

        return (HasLeadingComment(syntaxNode, "// csharpier-ignore"));
    }

    // TODO better name
    // TODO members of types
    // TODO these last two could get tricky because of the using vs members thing
    // unless we don't support using
    // TODO top level statements
    // TODO compilation unit
    public static List<Doc> GetPrintedNodes<T>(SyntaxList<T> list, FormattingContext context)
        where T : SyntaxNode
    {
        var statements = new List<Doc>();
        var printUnformatted = false;
        // TODO what about if the end is the final line of the block?
        // TODO what about any kind of error detection? if they have multiple starts? or other cases
        // TODO what about getting this into other areas? make it generic
        foreach (var node in list)
        {
            if (HasLeadingComment(node, "// csharpier-ignore-end"))
            {
                printUnformatted = false;
            }
            else if (HasLeadingComment(node, "// csharpier-ignore-start"))
            {
                printUnformatted = true;
            }

            statements.Add(
                printUnformatted ? PrintWithoutFormatting(node) : Node.Print(node, context)
            );
        }

        return statements;
    }

    private static bool HasLeadingComment(SyntaxNode node, string comment)
    {
        return node.GetLeadingTrivia()
            .Any(
                o =>
                    o.RawSyntaxKind() is SyntaxKind.SingleLineCommentTrivia
                    && o.ToString().Equals(comment)
            );
    }

    public static Doc PrintWithoutFormatting(SyntaxNode syntaxNode)
    {
        return syntaxNode.GetText().ToString().Trim();
    }
}
