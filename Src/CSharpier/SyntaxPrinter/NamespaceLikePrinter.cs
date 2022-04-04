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
                    docs.Add(Doc.HardLine);
                }
            }
            docs.Add(
                Doc.HardLine,
                AttributeLists.Print(node, compilationUnitSyntax.AttributeLists)
            );
        }

        if (members.Count <= 0)
        {
            return;
        }

        if (usings.Any() || (!usings.Any() && externs.Any()))
        {
            var directiveTrivia = members[0].GetLeadingTrivia().Where(o => o.IsDirective).ToArray();

            if (directiveTrivia.Any())
            {
                if (
                    (
                        node is not CompilationUnitSyntax { AttributeLists: { Count: > 0 } }
                        && directiveTrivia.All(o => o.IsKind(SyntaxKind.EndIfDirectiveTrivia))
                    ) || !directiveTrivia.All(o => o.IsKind(SyntaxKind.EndIfDirectiveTrivia))
                )
                {
                    docs.Add(ExtraNewLines.Print(members[0]));
                }
            }
            else if (node is not CompilationUnitSyntax { AttributeLists: { Count: > 0 } })
            {
                docs.Add(Doc.HardLine);
            }
        }

        docs.AddRange(MembersWithForcedLines.Print(node, members));
    }
}
