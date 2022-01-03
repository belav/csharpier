namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CompilationUnit
{
    public static Doc Print(CompilationUnitSyntax node)
    {
        var docs = new List<Doc>();

        NamespaceLikePrinter.Print(node, docs);

        var finalTrivia = Token.PrintLeadingTriviaWithNewLines(node.EndOfFileToken.LeadingTrivia);
        if (finalTrivia != Doc.Null)
        {
            // even though we include the initialNewLines above, a literalLine from directives trims the hardline, so add an extra one here
            docs.Add(Doc.HardLineIfNoPreviousLine, finalTrivia);
        }
        docs.Add(Doc.HardLineIfNoPreviousLine);

        return Doc.Concat(docs);
    }
}
