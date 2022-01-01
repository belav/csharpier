namespace CSharpier.SyntaxPrinter;

internal static class MembersWithForcedLines
{
    public static List<Doc> Print(
        CSharpSyntaxNode node,
        SyntaxList<MemberDeclarationSyntax> members
    )
    {
        var result = new List<Doc> { Doc.HardLine };
        var lastMemberForcedBlankLine = false;
        for (var x = 0; x < members.Count; x++)
        {
            var member = members[x];

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

            if (x == 0)
            {
                lastMemberForcedBlankLine = blankLineIsForced;
                result.Add(Node.Print(member));
                continue;
            }

            var addBlankLine = blankLineIsForced || lastMemberForcedBlankLine;

            if (!addBlankLine)
            {
                addBlankLine =
                    member.AttributeLists.Any()
                    || member
                        .GetLeadingTrivia()
                        .Any(o => o.Kind() is SyntaxKind.EndOfLineTrivia || o.IsComment());
            }

            if (addBlankLine)
            {
                result.Add(Doc.HardLine);
            }

            result.Add(Doc.HardLine, Node.Print(member));

            lastMemberForcedBlankLine = blankLineIsForced;
        }

        return result;
    }
}
