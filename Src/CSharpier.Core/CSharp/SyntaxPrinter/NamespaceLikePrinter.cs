using CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class NamespaceLikePrinter
{
    public static void Print(
        BaseNamespaceDeclarationSyntax node,
        List<Doc> docs,
        CSharpPrintingContext context
    )
    {
        Print(node, node.Externs, node.Usings, node.Members, docs, context);
    }

    public static void Print(
        CompilationUnitSyntax node,
        List<Doc> docs,
        CSharpPrintingContext context
    )
    {
        Print(node, node.Externs, node.Usings, node.Members, docs, context);
    }

    private static void Print(
        CSharpSyntaxNode node,
        SyntaxList<ExternAliasDirectiveSyntax> externs,
        SyntaxList<UsingDirectiveSyntax> usings,
        SyntaxList<MemberDeclarationSyntax> members,
        List<Doc> docs,
        CSharpPrintingContext context
    )
    {
        if (externs.Count > 0)
        {
            var externDocs = new Doc[(externs.Count * 2) - 1];
            for (var index = 0; index < externs.Count; index++)
            {
                if (index != 0)
                {
                    externDocs[(index * 2) - 1] = Doc.HardLine;
                }

                externDocs[index * 2] = ExternAliasDirective.Print(
                    externs[index],
                    context,
                    printExtraLines: index != 0
                );
            }

            docs.Add(Doc.Concat(externDocs));
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
                    compilationUnitSyntax.AttributeLists[0].GetLeadingTrivia().AnyDirective()
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

            if (directiveTrivia.Length != 0)
            {
                if (
                    (
                        node is not CompilationUnitSyntax { AttributeLists.Count: > 0 }
                        && directiveTrivia.All(o =>
                            o.RawSyntaxKind() is SyntaxKind.EndIfDirectiveTrivia
                        )
                    )
                    || !directiveTrivia.All(o =>
                        o.RawSyntaxKind() is SyntaxKind.EndIfDirectiveTrivia
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
