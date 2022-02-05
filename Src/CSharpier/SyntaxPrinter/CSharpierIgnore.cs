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

        return syntaxNode
            .GetLeadingTrivia()
            .Any(
                o =>
                    o.Kind() is SyntaxKind.SingleLineCommentTrivia
                    && o.ToString().Equals("// csharpier-ignore")
            );
    }

    public static Doc PrintWithoutFormatting(SyntaxNode syntaxNode)
    {
        return syntaxNode.GetText().ToString().Trim();
    }
}
