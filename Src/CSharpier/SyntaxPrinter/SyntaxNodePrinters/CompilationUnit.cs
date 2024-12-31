namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CompilationUnit
{
    public static Doc Print(CompilationUnitSyntax node, PrintingContext context)
    {
        var docs = new List<Doc>();

        NamespaceLikePrinter.Print(node, docs, context);

        var finalTrivia = Token.PrintLeadingTriviaWithNewLines(
            node.EndOfFileToken.LeadingTrivia,
            context
        );
        if (finalTrivia != Doc.Null)
        {
            // really ugly code to prevent a comment at the end of a file from continually inserting new blank lines
            if (
                finalTrivia is Concat { Contents.Count: > 1 } list
                && list.Contents[1] is LeadingComment
                && docs.Count > 0
                && docs[^1] is Concat { Contents.Count: > 1 } previousList
                && previousList.Contents[^1] is HardLine
                && previousList.Contents[^2] is HardLine
            )
            {
                list.Contents.RemoveAt(0);

                docs.Add(finalTrivia);
            }
            else
            {
                // even though we include the initialNewLines above, a literalLine from directives trims the hardline, so add an extra one here
                docs.Add(Doc.HardLineIfNoPreviousLine, finalTrivia);
            }
        }
        docs.Add(Doc.HardLineIfNoPreviousLine);

        return Doc.Concat(docs);
    }
}
