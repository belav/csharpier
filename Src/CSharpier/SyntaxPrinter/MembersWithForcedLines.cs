namespace CSharpier.SyntaxPrinter;

internal static class MembersWithForcedLines
{
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

            if (!addBlankLine)
            {
                addBlankLine =
                    member.AttributeLists.Any()
                    || (
                        member
                            .GetLeadingTrivia()
                            .Any(
                                o =>
                                    o.RawSyntaxKind() is SyntaxKind.EndOfLineTrivia || o.IsComment()
                            )
                    );
            }

            if (
                member
                    .GetLeadingTrivia()
                    .Any(
                        o =>
                            o.RawSyntaxKind()
                                is SyntaxKind.PragmaWarningDirectiveTrivia
                                    or SyntaxKind.PragmaChecksumDirectiveTrivia
                                    or SyntaxKind.IfDirectiveTrivia
                                    or SyntaxKind.EndRegionDirectiveTrivia
                    )
            )
            {
                result.Add(ExtraNewLines.Print(member));
            }
            else if (
                addBlankLine
                && !member
                    .GetLeadingTrivia()
                    .Any(
                        o =>
                            o.RawSyntaxKind()
                                is SyntaxKind.EndIfDirectiveTrivia
                                    or SyntaxKind.EndRegionDirectiveTrivia
                    )
            )
            {
                result.Add(Doc.HardLine);
            }

            result.Add(Doc.HardLine, Node.Print(member));
            AddSeparatorIfNeeded();

            lastMemberForcedBlankLine = blankLineIsForced;
        }

        return result;
    }
}
