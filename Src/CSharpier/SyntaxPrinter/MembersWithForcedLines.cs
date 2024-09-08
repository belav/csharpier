using System.Collections.Immutable;

namespace CSharpier.SyntaxPrinter;

using System.Text;

internal static class MembersWithForcedLines
{
    public static List<Doc> Print<T>(
        CSharpSyntaxNode node,
        IReadOnlyList<T> members,
        FormattingContext context,
        bool skipFirstHardLine = false
    )
        where T : MemberDeclarationSyntax
    {
        var result = new List<Doc>();
        if (!skipFirstHardLine)
        {
            result.Add(Doc.HardLine);
        }

        var unFormattedCode = new StringBuilder();
        var printUnformatted = false;
        var lastMemberForcedBlankLine = false;
        for (var memberIndex = 0; memberIndex < members.Count; memberIndex++)
        {
            var skipAddingLineBecauseIgnoreEnded = false;
            var member = members[memberIndex];

            if (Token.HasLeadingCommentMatching(member, CSharpierIgnore.IgnoreEndRegex))
            {
                skipAddingLineBecauseIgnoreEnded = true;
                result.Add(unFormattedCode.ToString().Trim());
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

            void AddSeparatorIfNeeded()
            {
                if (members is SeparatedSyntaxList<T> list)
                {
                    if (memberIndex < list.SeparatorCount)
                    {
                        result.Add(Token.Print(list.GetSeparator(memberIndex), context));
                    }
                    else if (
                        node is EnumDeclarationSyntax enumDeclarationSyntax
                        && member is EnumMemberDeclarationSyntax
                    )
                    {
                        result.Add(
                            TrailingComma.Print(enumDeclarationSyntax.CloseBraceToken, context)
                        );
                    }
                }
            }

            var blankLineIsForced = (
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
                        or StructDeclarationSyntax
            );

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
                AddSeparatorIfNeeded();
                continue;
            }

            var addBlankLine = (blankLineIsForced || lastMemberForcedBlankLine);

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
                context.NextTriviaNeedsLine = true;
            }

            result.Add(Doc.HardLine, Node.Print(member, context));
            AddSeparatorIfNeeded();

            lastMemberForcedBlankLine = blankLineIsForced;
        }

        if (unFormattedCode.Length > 0)
        {
            result.Add(unFormattedCode.ToString().Trim());
        }

        return result;
    }
}
