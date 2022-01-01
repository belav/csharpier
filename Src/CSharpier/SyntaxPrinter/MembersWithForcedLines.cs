namespace CSharpier.SyntaxPrinter;

internal static class MembersWithForcedLines
{
    public static List<Doc> Print(
        CSharpSyntaxNode node,
        SyntaxList<MemberDeclarationSyntax> members
    )
    {
        var result = new List<Doc> { Doc.HardLine };
        var lastMemberForcedLine = false;
        for (var x = 0; x < members.Count; x++)
        {
            var member = members[x];

            var lineIsForced =
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
                lastMemberForcedLine = lineIsForced;
                result.Add(Node.Print(member));
                continue;
            }

            var addLine = lineIsForced || lastMemberForcedLine;

            if (!addLine)
            {
                addLine =
                    member.AttributeLists.Any()
                    || member
                        .GetLeadingTrivia()
                        .Any(o => o.Kind() is SyntaxKind.EndOfLineTrivia || o.IsComment());
            }

            result.Add(Doc.HardLine);

            if (addLine)
            {
                result.Add(Doc.HardLine);
            }

            result.Add(Node.Print(member));

            lastMemberForcedLine = lineIsForced;
        }

        return result;
    }
}
