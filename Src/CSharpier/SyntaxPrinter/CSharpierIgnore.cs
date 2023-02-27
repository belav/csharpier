namespace CSharpier.SyntaxPrinter;

using System.Text;
using System.Text.RegularExpressions;

internal static class CSharpierIgnore
{
    private static readonly Regex IgnoreRegex = new("^// csharpier-ignore($| -)");
    public static readonly Regex IgnoreStartRegex = new("^// csharpier-ignore-start($| -)");
    public static readonly Regex IgnoreEndRegex = new("^// csharpier-ignore-end($| -)");

    public static bool IsNodeIgnored(SyntaxNode syntaxNode)
    {
        return syntaxNode.Parent
                is BaseTypeDeclarationSyntax
                    or BlockSyntax
                    or CompilationUnitSyntax
                    or NamespaceDeclarationSyntax
            && Token.HasLeadingCommentMatching(syntaxNode, IgnoreRegex);
    }

    public static List<Doc> PrintNodesRespectingRangeIgnore<T>(
        SyntaxList<T> list,
        FormattingContext context
    )
        where T : SyntaxNode
    {
        var statements = new List<Doc>();
        var unFormattedCode = new StringBuilder();
        var printUnformatted = false;

        foreach (var node in list)
        {
            if (Token.HasLeadingCommentMatching(node, IgnoreEndRegex))
            {
                statements.Add(unFormattedCode.ToString().Trim());
                unFormattedCode.Clear();
                printUnformatted = false;
            }
            else if (Token.HasLeadingCommentMatching(node, IgnoreStartRegex))
            {
                printUnformatted = true;
            }

            if (printUnformatted)
            {
                unFormattedCode.Append(PrintWithoutFormatting(node, context));
            }
            else
            {
                statements.Add(Node.Print(node, context));
            }
        }

        if (unFormattedCode.Length > 0)
        {
            statements.Add(unFormattedCode.ToString().Trim());
        }

        return statements;
    }

    public static string PrintWithoutFormatting(SyntaxNode syntaxNode, FormattingContext context)
    {
        return syntaxNode.GetText().ToString().TrimEnd() + context.LineEnding;
    }
}
