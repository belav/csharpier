using System.Collections.Immutable;

namespace CSharpier.SyntaxPrinter;

internal static class MembersWithForcedLines
{
    // TODO some edgecases to fix, see https://github.com/belav/csharpier-repos/pull/40/files
    /*
     #if INTERNAL_ASYMMETRIC_IMPLEMENTATIONS
public class ClassName
{
    public void MethodName { }
#endif
    public class NestedClass
    {
        private int field;
    }
#if INTERNAL_ASYMMETRIC_IMPLEMENTATIONS
}
#endif

     
    start from https://github.com/belav/csharpier-repos/pull/40/files#diff-cfd2c2802b10c6701ade623e79a187812b6ba0d69baa6e4a934e23354e82c103
    
     */

    public static List<Doc> Print<T>(CSharpSyntaxNode node, IReadOnlyList<T> members)
        where T : MemberDeclarationSyntax
    {
        var result = new List<Doc> { Doc.HardLine };
        var lastMemberForcedBlankLine = false;
        for (var x = 0; x < members.Count; x++)
        {
            void AddSeparatorIfNeeded()
            {
                if (members is SeparatedSyntaxList<T> list && x < list.SeparatorCount)
                {
                    result.Add(Token.Print(list.GetSeparator(x)));
                }
            }

            var member = members[x];

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
                && classDeclaration.Modifiers.Any(
                    o => o.RawSyntaxKind() is SyntaxKind.AbstractKeyword
                )
                && methodDeclaration.Modifiers.Any(
                    o => o.RawSyntaxKind() is SyntaxKind.AbstractKeyword
                )
            )
            {
                blankLineIsForced = false;
            }

            if (x == 0)
            {
                lastMemberForcedBlankLine = blankLineIsForced;
                result.Add(Node.Print(member));
                AddSeparatorIfNeeded();
                continue;
            }

            var addBlankLine = blankLineIsForced || lastMemberForcedBlankLine;

            var triviaContainsCommentOrNewLine = false;
            var printExtraNewLines = false;
            var triviaContainsEndIfOrRegion = false;

            // TODO test if this even matters, or if it makes memory worse
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
                Token.NextTriviaNeedsLine = true;
            }
            else if (addBlankLine && !triviaContainsEndIfOrRegion)
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
                Token.NextTriviaNeedsLine = true;
            }

            result.Add(Doc.HardLine, Node.Print(member));
            AddSeparatorIfNeeded();

            lastMemberForcedBlankLine = blankLineIsForced;
        }

        return result;
    }
}
