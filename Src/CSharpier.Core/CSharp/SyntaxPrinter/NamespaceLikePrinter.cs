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

        var skipFirstHardLine = !usings.Any() && !externs.Any() && !isCompilationUnitWithAttributes;
        var leadingTrivia = members[0].GetLeadingTrivia();

        if (isCompilationUnitWithAttributes)
        {
            if (FirstDirectiveIsClosing(leadingTrivia))
            {
                docs.Add(ExtraNewLines.Print(members[0]));
                skipFirstHardLine = true;

                if (NeedsLineAfterClosingDirective(leadingTrivia))
                {
                    context.State.NextTriviaNeedsLine = true;
                }
            }
        }
        else if (usings.Any() || externs.Any())
        {
            docs.Add(
                leadingTrivia.Any(o => o.IsDirective)
                    ? ExtraNewLines.Print(members[0])
                    : Doc.HardLine
            );
        }

        docs.AddRange(MembersWithForcedLines.Print(node, members, context, skipFirstHardLine));
    }

    private static bool FirstDirectiveIsClosing(SyntaxTriviaList leadingTrivia)
    {
        foreach (var trivia in leadingTrivia)
        {
            if (trivia.IsDirective)
            {
                return trivia.RawSyntaxKind()
                    is SyntaxKind.ElifDirectiveTrivia
                        or SyntaxKind.ElseDirectiveTrivia
                        or SyntaxKind.EndIfDirectiveTrivia
                        or SyntaxKind.EndRegionDirectiveTrivia;
            }
        }

        return false;
    }

    private static bool NeedsLineAfterClosingDirective(SyntaxTriviaList leadingTrivia)
    {
        var lastDirectiveIndex = -1;
        for (var index = 0; index < leadingTrivia.Count; index++)
        {
            if (leadingTrivia[index].RawSyntaxKind() is SyntaxKind.IfDirectiveTrivia)
            {
                return false;
            }

            if (leadingTrivia[index].IsDirective)
            {
                lastDirectiveIndex = index;
            }
        }

        return lastDirectiveIndex >= 0
            && leadingTrivia[lastDirectiveIndex].RawSyntaxKind()
                is SyntaxKind.EndIfDirectiveTrivia
                    or SyntaxKind.EndRegionDirectiveTrivia
            && leadingTrivia.IndexOfNextEndOfLine(lastDirectiveIndex) < 0;
    }
}
