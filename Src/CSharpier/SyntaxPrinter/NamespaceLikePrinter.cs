namespace CSharpier.SyntaxPrinter;

internal static class NamespaceLikePrinter
{
    public static void Print(
        BaseNamespaceDeclarationSyntax node,
        List<Doc> docs,
        FormattingContext context
    )
    {
        Print(node, node.Externs, node.Usings, node.Members, docs, context);
    }

    public static void Print(CompilationUnitSyntax node, List<Doc> docs, FormattingContext context)
    {
        Print(node, node.Externs, node.Usings, node.Members, docs, context);
    }

    private static void Print(
        CSharpSyntaxNode node,
        SyntaxList<ExternAliasDirectiveSyntax> externs,
        SyntaxList<UsingDirectiveSyntax> usings,
        SyntaxList<MemberDeclarationSyntax> members,
        List<Doc> docs,
        FormattingContext context
    )
    {
        if (externs.Count > 0)
        {
            docs.Add(
                Doc.Join(
                    Doc.HardLine,
                    externs.Select(
                        (o, i) => ExternAliasDirective.Print(o, context, printExtraLines: i != 0)
                    )
                )
            );
        }

        if (usings.Count > 0)
        {
            if (externs.Count > 0)
            {
                docs.Add(Doc.HardLine);
            }

            docs.Add(UsingDirectives.PrintWithSorting(usings, context, externs.Count != 0));
        }

        var isCompilationUnitWithAttributes = false;

        if (
            node is CompilationUnitSyntax compilationUnitSyntax
            && compilationUnitSyntax.AttributeLists.Any()
        )
        {
            isCompilationUnitWithAttributes = true;

            if (externs.Any() || usings.Any())
            {
                docs.Add(
                    compilationUnitSyntax
                        .AttributeLists[0]
                        .GetLeadingTrivia()
                        .Any(o => o.IsDirective)
                        ? ExtraNewLines.Print(compilationUnitSyntax.AttributeLists[0])
                        : Doc.HardLine
                );
            }
            docs.Add(
                Doc.HardLine,
                AttributeLists.Print(node, compilationUnitSyntax.AttributeLists, context)
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
                        node is not CompilationUnitSyntax { AttributeLists.Count: > 0 }
                        && directiveTrivia.All(
                            o => o.RawSyntaxKind() is SyntaxKind.EndIfDirectiveTrivia
                        )
                    )
                    || !directiveTrivia.All(
                        o => o.RawSyntaxKind() is SyntaxKind.EndIfDirectiveTrivia
                    )
                )
                {
                    docs.Add(ExtraNewLines.Print(members[0]));
                }
            }
            else if (node is not CompilationUnitSyntax { AttributeLists.Count: > 0 })
            {
                docs.Add(Doc.HardLine);
            }
        }

        docs.AddRange(
            MembersWithForcedLines.Print(
                node,
                members,
                context,
                skipFirstHardLine: !usings.Any()
                    && !externs.Any()
                    && !isCompilationUnitWithAttributes
            )
        );
    }
}
