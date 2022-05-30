namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class ListPattern
{
    public static Doc Print(ListPatternSyntax node, FormattingContext context)
    {
        // TODO 11 are there list pattern edge cases?
        // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns#list-patterns
        return node.ToString();
    }
}
