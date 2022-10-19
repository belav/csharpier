namespace CSharpier.SyntaxPrinter;

internal static class Modifiers
{
    public static Doc Print(SyntaxTokenList modifiers, FormattingContext context)
    {
        if (modifiers.Count == 0)
        {
            return Doc.Null;
        }

        return Doc.Group(Doc.Join(" ", modifiers.Select(o => Token.Print(o, context))), " ");
    }

    public static Doc PrintWithoutLeadingTrivia(
        SyntaxTokenList modifiers,
        FormattingContext context
    )
    {
        if (modifiers.Count == 0)
        {
            return Doc.Null;
        }

        var sortedModifiers = modifiers.OrderBy(o => o.Text);

        return Doc.Group(
            Token.PrintWithoutLeadingTrivia(sortedModifiers.First(), context),
            " ",
            sortedModifiers.Count() > 1
                ? Doc.Concat(
                    sortedModifiers.Skip(1).Select(o => Token.PrintWithSuffix(o, " ", context)).ToArray()
                )
                : Doc.Null
        );
    }
}
