using System.Collections.Immutable;

namespace CSharpier.SyntaxPrinter;

internal static class MembersWithForcedLines
{
    // TODO some edgecases to fix, see https://github.com/belav/csharpier-repos/pull/40/files
    // one of them is
/*
public class ClassName
{
#if TODO


    public void RemoveLinesAbove() { }
#endif
    /// <summary>Summary/summary>
    public async Task AddLineAboveSummary() { }
}

        private sealed class WorkStealingQueue
        {
            /// <summary>Initial size of the queue's array.</summary>
            private const int InitialSize = 32;
            /// <summary>Starting index for the head and tail indices.</summary>
            private const int StartIndex =
#if DEBUG
            int.MaxValue; // in debug builds, start at the end so we exercise the index reset logic
#else
                0;
#endif
            /// <summary>Head index from which to steal.  This and'd with the <see cref="_mask"/> is the index into <see cref="_array"/>.</summary>
            private volatile int _headIndex = StartIndex
            
https://github.com/belav/csharpier-repos/pull/40/files#diff-7a8318af9bf1a68826755c4ef53496161290589ce1cc1edee671923480605915

start from https://github.com/belav/csharpier-repos/pull/40/files#diff-cfd2c2802b10c6701ade623e79a187812b6ba0d69baa6e4a934e23354e82c103

// TODO https://raw.githubusercontent.com/belav/csharpier-repos/8c770abc7626f3eb2dcbe416f2728c5cd36abfba/runtime/src/libraries/Common/src/System/Security/Cryptography/ECDsaSecurityTransforms.cs
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
                result.Add(ExtraNewLines.Print(member));
            }
            else if (addBlankLine && !triviaContainsEndIfOrRegion)
            {
                result.Add(Doc.HardLine);
            }

            // TODO clean this up, also some type of context should be passed around so that thread statics aren't necessary
            if (
                addBlankLine
                && !triviaContainsEndIfOrRegion
                && leadingTrivia.Contains(SyntaxKind.IfDirectiveTrivia)
                && !leadingTrivia.Contains(SyntaxKind.EndOfLineTrivia)
            )
            {
                Token.NextTriviaNeedsLine = true;
            }
            else if (
                addBlankLine
                && triviaContainsEndIfOrRegion
                && !leadingTrivia.Contains(SyntaxKind.IfDirectiveTrivia)
                && !leadingTrivia.Contains(SyntaxKind.ElifDirectiveTrivia)
                && !leadingTrivia.Contains(SyntaxKind.ElseDirectiveTrivia)
                && !leadingTrivia.Contains(SyntaxKind.EndOfLineTrivia)
                && !printExtraNewLines
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
