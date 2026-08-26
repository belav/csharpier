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
        CSharpPrintingContext context,
        bool skipFirstHardLine = false
    )
        where T : MemberDeclarationSyntax
    {
        var result = new List<Doc>(members.Count * 3);
        if (!skipFirstHardLine)
        {
            result.Add(Doc.HardLine);
        }

        StringBuilder? unFormattedCode = null;
        var printUnformatted = false;
        var lastMemberForcedBlankLine = false;
        for (var memberIndex = 0; memberIndex < members.Count; memberIndex++)
        {
            var skipAddingLineBecauseIgnoreEnded = false;
            var member = members[memberIndex];
            // GetLeadingTrivia walks the left hand spine of the member, so get it once
            var leadingTrivia = member.GetLeadingTrivia();

            if (Token.HasLeadingCommentMatching(leadingTrivia, CSharpierIgnore.IgnoreEndRegex))
            {
                skipAddingLineBecauseIgnoreEnded = true;
                result.Add(unFormattedCode?.ToString().Trim() ?? string.Empty);
                unFormattedCode?.Clear();
                printUnformatted = false;
            }
            else if (
                Token.HasLeadingCommentMatching(leadingTrivia, CSharpierIgnore.IgnoreStartRegex)
            )
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
                unFormattedCode ??= new StringBuilder();
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
            var triviaContainsIfDirective = false;
            var triviaContainsElifDirective = false;
            var triviaContainsElseDirective = false;
            var triviaContainsEndOfLine = false;
            var triviaContainsSingleLineComment = false;

            foreach (var trivia in leadingTrivia)
            {
                var syntaxTrivia = trivia.RawSyntaxKind();

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
                // EndRegionDirectiveTrivia is matched by the arm above as well, so only
                // EndIfDirectiveTrivia ever reaches here. that is long standing behavior, so the
                // label stays rather than quietly changing what gets printed
                else if (
                    syntaxTrivia
                    is SyntaxKind.EndIfDirectiveTrivia
                        or SyntaxKind.EndRegionDirectiveTrivia
                )
                {
                    triviaContainsEndIfOrRegion = true;
                }

                if (syntaxTrivia is SyntaxKind.IfDirectiveTrivia)
                {
                    triviaContainsIfDirective = true;
                }
                else if (syntaxTrivia is SyntaxKind.ElifDirectiveTrivia)
                {
                    triviaContainsElifDirective = true;
                }
                else if (syntaxTrivia is SyntaxKind.ElseDirectiveTrivia)
                {
                    triviaContainsElseDirective = true;
                }
                else if (syntaxTrivia is SyntaxKind.EndOfLineTrivia)
                {
                    triviaContainsEndOfLine = true;
                }
                else if (syntaxTrivia is SyntaxKind.SingleLineCommentTrivia)
                {
                    triviaContainsSingleLineComment = true;
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
                        && triviaContainsIfDirective
                        && !triviaContainsEndOfLine
                    )
                    || (
                        triviaContainsEndIfOrRegion
                        && !triviaContainsIfDirective
                        && !triviaContainsElifDirective
                        && !triviaContainsElseDirective
                        // single comments have an EndOfLine separate
                        // ideally we would just exclude if leadingTrivia contains EndOfLineTrivia
                        && (!triviaContainsEndOfLine || triviaContainsSingleLineComment)
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

        if (unFormattedCode is { Length: > 0 })
        {
            result.Add(unFormattedCode.ToString().Trim());
        }

        return result;
    }
}
