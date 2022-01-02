namespace CSharpier.SyntaxPrinter;

internal static class NamespaceLikePrinter
{
    public static void Print(BaseNamespaceDeclarationSyntax node, List<Doc> docs)
    {
        Print(node, node.Externs, node.Usings, node.Members, docs);
    }

    public static void Print(CompilationUnitSyntax node, List<Doc> docs)
    {
        Print(node, node.Externs, node.Usings, node.Members, docs);
    }

    private static void Print(
        CSharpSyntaxNode node,
        SyntaxList<ExternAliasDirectiveSyntax> externs,
        SyntaxList<UsingDirectiveSyntax> usings,
        SyntaxList<MemberDeclarationSyntax> members,
        List<Doc> docs
    )
    {
        if (externs.Count > 0)
        {
            docs.Add(Doc.Join(Doc.HardLine, externs.Select(ExternAliasDirective.Print)));
        }

        if (usings.Count > 0)
        {
            if (externs.Any())
            {
                docs.Add(Doc.HardLine);
            }
            docs.Add(Doc.Join(Doc.HardLine, usings.Select(UsingDirective.Print)));
        }

        if (
            node is CompilationUnitSyntax compilationUnitSyntax
            && compilationUnitSyntax.AttributeLists.Any()
        )
        {
            if (externs.Any() || usings.Any())
            {
                if (
                    compilationUnitSyntax.AttributeLists[0]
                        .GetLeadingTrivia()
                        .Any(o => o.IsDirective)
                )
                {
                    docs.Add(ExtraNewLines.Print(compilationUnitSyntax.AttributeLists[0]));
                }
                else
                {
                    docs.Add(Doc.HardLine, Doc.HardLine);
                }
            }
            docs.Add(AttributeLists.Print(node, compilationUnitSyntax.AttributeLists));
        }

        if (members.Count <= 0)
        {
            return;
        }

        if (usings.Any() || (!usings.Any() && externs.Any()))
        {
            if (members[0].GetLeadingTrivia().Any(o => o.IsDirective))
            {
                docs.Add(ExtraNewLines.Print(members[0]));
            }
            else
            {
                docs.Add(Doc.HardLine);
            }
        }
        docs.AddRange(MembersWithForcedLines.Print(node, members));
    }
}
