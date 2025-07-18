using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;
using CSharpier.Core.DocTypes;
using CSharpier.Core.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter;

internal static class MembersWithForcedLines
{
    [SkipLocalsInit]
    public static List<Doc> Print<T>(
        CSharpSyntaxNode node,
        IReadOnlyList<T> members,
        PrintingContext context,
        bool skipFirstHardLine = false
    )
        where T : MemberDeclarationSyntax
    {
        var result = new List<Doc>();
        if (!skipFirstHardLine)
        {
            result.Add(Doc.HardLine);
        }

        var unFormattedCode = new ValueListBuilder<char>(stackalloc char[64]);
        var printUnformatted = false;
        var lastMemberForcedBlankLine = false;
        for (var memberIndex = 0; memberIndex < members.Count; memberIndex++)
        {
            var skipAddingLineBecauseIgnoreEnded = false;
            var member = members[memberIndex];

            if (Token.HasLeadingCommentMatching(member, CSharpierIgnore.IgnoreEndRegex))
            {
                skipAddingLineBecauseIgnoreEnded = true;
                result.Add(unFormattedCode.AsSpan().Trim().ToString());
                unFormattedCode.Clear();
                printUnformatted = false;
            }
            else if (Token.HasLeadingCommentMatching(member, CSharpierIgnore.IgnoreStartRegex))
            {
                if (!printUnformatted && memberIndex > 0)
                {
                    result.Add(Doc.HardLine);
                    result.Add(ExtraNewLines.Print(member));
                }
                printUnformatted = true;
            }

            if (printUnformatted)
            {
                unFormattedCode.Append(CSharpierIgnore.PrintWithoutFormatting(member, context));
                continue;
            }

            Doc GetSeparatorIfNeeded()
            {
                if (members is not SeparatedSyntaxList<T> list)
                {
                    return Doc.Null;
                }

                if (memberIndex < list.SeparatorCount)
                {
                    return Token.Print(list.GetSeparator(memberIndex), context);
                }

                if (
                    node is EnumDeclarationSyntax enumDeclarationSyntax
                    && member is EnumMemberDeclarationSyntax
                )
                {
                    var firstTrailingComment = list[memberIndex]
                        .GetTrailingTrivia()
                        .FirstOrDefault(o => o.IsComment());

                    if (firstTrailingComment != default)
                    {
                        context.WithTrailingComma(
                            firstTrailingComment,
                            TrailingComma.Print(
                                enumDeclarationSyntax.CloseBraceToken,
                                context,
                                true
                            )
                        );
                    }
                    else
                    {
                        return TrailingComma.Print(enumDeclarationSyntax.CloseBraceToken, context);
                    }
                }

                return Doc.Null;
            }

            var blankLineIsForced =
                member is MethodDeclarationSyntax && node is not InterfaceDeclarationSyntax
                || member
                    is ClassDeclarationSyntax
                        or ConstructorDeclarationSyntax
                        or ConversionOperatorDeclarationSyntax
                        or DestructorDeclarationSyntax
                        or EnumDeclarationSyntax
                        or FileScopedNamespaceDeclarationSyntax
                        or InterfaceDeclarationSyntax
                        or NamespaceDeclarationSyntax
                        or OperatorDeclarationSyntax
                        or RecordDeclarationSyntax
                        or StructDeclarationSyntax;

            if (
                member is MethodDeclarationSyntax methodDeclaration
                && node is ClassDeclarationSyntax classDeclaration
                && classDeclaration.Modifiers.Any(o =>
                    o.RawSyntaxKind() is SyntaxKind.AbstractKeyword
                )
                && methodDeclaration.Modifiers.Any(o =>
                    o.RawSyntaxKind() is SyntaxKind.AbstractKeyword
                )
            )
            {
                blankLineIsForced = false;
            }

            if (memberIndex == 0)
            {
                lastMemberForcedBlankLine = blankLineIsForced;
                result.Add(Node.Print(member, context));
                result.AddIfNotNull(GetSeparatorIfNeeded());

                continue;
            }

            var addBlankLine = blankLineIsForced || lastMemberForcedBlankLine;

            var triviaContainsCommentOrNewLine = false;
            var printExtraNewLines = false;
            var triviaContainsEndIfOrRegion = false;

            var leadingTrivia = member
                .GetLeadingTrivia()
                .Select(o => o.RawSyntaxKind())
                .ToImmutableHashSet();

            foreach (var syntaxTrivia in leadingTrivia)
            {
                if (syntaxTrivia is SyntaxKind.EndOfLineTrivia || syntaxTrivia.IsComment())
                {
                    triviaContainsCommentOrNewLine = true;
                }
                else if (
                    syntaxTrivia
                    is SyntaxKind.PragmaWarningDirectiveTrivia
                        or SyntaxKind.PragmaChecksumDirectiveTrivia
                        or SyntaxKind.IfDirectiveTrivia
                        or SyntaxKind.EndRegionDirectiveTrivia
                )
                {
                    printExtraNewLines = true;
                }
                else if (
                    syntaxTrivia
                    is SyntaxKind.EndIfDirectiveTrivia
                        or SyntaxKind.EndRegionDirectiveTrivia
                )
                {
                    triviaContainsEndIfOrRegion = true;
                }
            }

            if (!addBlankLine)
            {
                addBlankLine = member.AttributeLists.Any() || triviaContainsCommentOrNewLine;
            }

            if (printExtraNewLines)
            {
                result.Add(ExtraNewLines.Print(member));
            }
            else if (
                addBlankLine
                && !triviaContainsEndIfOrRegion
                && !skipAddingLineBecauseIgnoreEnded
            )
            {
                result.Add(Doc.HardLine);
            }

            // this handles inserting a new line after directives but before
            // comments on members. The directives are printed by Token, so we can't
            // directly print them here
            if (
                addBlankLine
                && (
                    (
                        !triviaContainsEndIfOrRegion
                        && leadingTrivia.Contains(SyntaxKind.IfDirectiveTrivia)
                        && !leadingTrivia.Contains(SyntaxKind.EndOfLineTrivia)
                    )
                    || (
                        triviaContainsEndIfOrRegion
                        && !leadingTrivia.Contains(SyntaxKind.IfDirectiveTrivia)
                        && !leadingTrivia.Contains(SyntaxKind.ElifDirectiveTrivia)
                        && !leadingTrivia.Contains(SyntaxKind.ElseDirectiveTrivia)
                        // single comments have an EndOfLine separate
                        // ideally we would just exclude if leadingTrivia contains EndOfLineTrivia
                        && (
                            !leadingTrivia.Contains(SyntaxKind.EndOfLineTrivia)
                            || leadingTrivia.Contains(SyntaxKind.SingleLineCommentTrivia)
                        )
                        && !printExtraNewLines
                    )
                )
            )
            {
                context.State.NextTriviaNeedsLine = true;
            }

            // this has a side effect (yuck) that fixes the trailing comma + trailing comment issue so we have to call it first
            var separator = GetSeparatorIfNeeded();
            result.Add(Doc.HardLine, Node.Print(member, context));
            result.AddIfNotNull(separator);

            lastMemberForcedBlankLine = blankLineIsForced;
        }

        if (unFormattedCode.Length > 0)
        {
            result.Add(unFormattedCode.AsSpan().ToString().Trim());
        }

        unFormattedCode.Dispose();

        return result;
    }
}
