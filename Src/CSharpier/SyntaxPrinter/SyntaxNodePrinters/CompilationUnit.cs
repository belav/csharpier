namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class CompilationUnit
{
    public static Doc Print(CompilationUnitSyntax node)
    {
        var docs = new List<Doc>();
        if (node.Externs.Count > 0)
        {
            docs.Add(
                Doc.Join(Doc.HardLine, node.Externs.Select(ExternAliasDirective.Print)),
                Doc.HardLine
            );
        }
        if (node.Usings.Count > 0)
        {
            docs.Add(
                Doc.Join(Doc.HardLine, node.Usings.Select(UsingDirective.Print)),
                Doc.HardLine
            );
        }
        docs.Add(AttributeLists.Print(node, node.AttributeLists));
        if (node.Members.Count > 0)
        {
            docs.Add(Doc.Join(Doc.HardLine, node.Members.Select(Node.Print)));
        }

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
