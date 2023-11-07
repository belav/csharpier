namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class BaseTypeDeclaration
{
    public static Doc Print(BaseTypeDeclarationSyntax node, FormattingContext context)
    {
        ParameterListSyntax? parameterList = null;
        TypeParameterListSyntax? typeParameterList = null;
        var constraintClauses = Enumerable.Empty<TypeParameterConstraintClauseSyntax>();
        SyntaxToken? recordKeyword = null;
        SyntaxToken? keyword = null;
        Func<Doc>? members = null;
        SyntaxToken? semicolonToken = null;

        if (node is TypeDeclarationSyntax typeDeclarationSyntax)
        {
            typeParameterList = typeDeclarationSyntax.TypeParameterList;
            constraintClauses = typeDeclarationSyntax.ConstraintClauses;
            if (typeDeclarationSyntax.Members.Count > 0)
            {
                members = () =>
                    Doc.Indent(
                        MembersWithForcedLines.Print(
                            typeDeclarationSyntax,
                            typeDeclarationSyntax.Members,
                            context
                        )
                    );
            }
            if (node is ClassDeclarationSyntax classDeclarationSyntax)
            {
                keyword = classDeclarationSyntax.Keyword;
                parameterList = classDeclarationSyntax.ParameterList;
            }
            else if (node is StructDeclarationSyntax structDeclarationSyntax)
            {
                keyword = structDeclarationSyntax.Keyword;
                parameterList = structDeclarationSyntax.ParameterList;
            }
            else if (node is InterfaceDeclarationSyntax interfaceDeclarationSyntax)
            {
                keyword = interfaceDeclarationSyntax.Keyword;
            }
            else if (node is RecordDeclarationSyntax recordDeclarationSyntax)
            {
                recordKeyword = recordDeclarationSyntax.Keyword;
                keyword = recordDeclarationSyntax.ClassOrStructKeyword;
                parameterList = recordDeclarationSyntax.ParameterList;
            }

            semicolonToken = typeDeclarationSyntax.SemicolonToken;
        }
        else if (node is EnumDeclarationSyntax enumDeclarationSyntax)
        {
            if (enumDeclarationSyntax.Members.Count > 0)
            {
                members = () =>
                    Doc.Indent(
                        MembersWithForcedLines.Print(
                            enumDeclarationSyntax,
                            enumDeclarationSyntax.Members,
                            context
                        )
                    );
            }

            keyword = enumDeclarationSyntax.EnumKeyword;
            semicolonToken = enumDeclarationSyntax.SemicolonToken;
        }

        var docs = new List<Doc>();
        if (node.AttributeLists.Any())
        {
            docs.Add(AttributeLists.Print(node, node.AttributeLists, context));
        }

        if (node.Modifiers.Any())
        {
            docs.Add(Modifiers.Print(node.Modifiers, context));
        }

        if (recordKeyword != null)
        {
            docs.Add(Token.PrintWithSuffix(recordKeyword.Value, " ", context));
        }

        if (keyword != null)
        {
            docs.Add(Token.PrintWithSuffix(keyword.Value, " ", context));
        }

        docs.Add(Token.Print(node.Identifier, context));

        if (typeParameterList != null)
        {
            docs.Add(TypeParameterList.Print(typeParameterList, context));
        }

        if (parameterList != null)
        {
            docs.Add(ParameterList.Print(parameterList, context));
        }

        if (node.BaseList != null)
        {
            var baseListDoc = Doc.Concat(
                Token.Print(node.BaseList.ColonToken, context),
                " ",
                Node.Print(node.BaseList.Types.First(), context),
                node.BaseList.Types.Count > 1
                    ? Doc.Indent(
                        Token.Print(node.BaseList.Types.GetSeparator(0), context),
                        Doc.Line,
                        SeparatedSyntaxList.Print(
                            node.BaseList.Types,
                            Node.Print,
                            Doc.Line,
                            context,
                            startingIndex: 1
                        )
                    )
                    : Doc.Null
            );

            docs.Add(Doc.Group(Doc.Indent(Doc.Line, baseListDoc)));
        }

        docs.Add(ConstraintClauses.Print(constraintClauses, context));

        if (members != null)
        {
            var membersContent = members();

            DocUtilities.RemoveInitialDoubleHardLine(membersContent);

            docs.Add(
                Doc.HardLine,
                Token.Print(node.OpenBraceToken, context),
                membersContent,
                Doc.HardLine,
                Token.Print(node.CloseBraceToken, context)
            );
        }
        else if (node.OpenBraceToken.RawSyntaxKind() != SyntaxKind.None)
        {
            Doc separator = node.CloseBraceToken
                .LeadingTrivia
                .Any(
                    o =>
                        o.RawSyntaxKind()
                            is not (SyntaxKind.WhitespaceTrivia or SyntaxKind.EndOfLineTrivia)
                )
                ? Doc.Line
                : " ";

            docs.Add(
                separator,
                Token.Print(node.OpenBraceToken, context),
                separator,
                Token.Print(node.CloseBraceToken, context)
            );
        }

        if (semicolonToken.HasValue)
        {
            docs.Add(Token.Print(semicolonToken.Value, context));
        }

        return Doc.Concat(docs);
    }
}
