using System;
using System.Collections.Generic;
using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBaseTypeDeclarationSyntax(
            BaseTypeDeclarationSyntax node
        ) {
            ParameterListSyntax? parameterList = null;
            TypeParameterListSyntax? typeParameterList = null;
            var constraintClauses = Enumerable.Empty<TypeParameterConstraintClauseSyntax>();
            var hasMembers = false;
            SyntaxToken? keyword = null;
            Doc members = Doc.Null;
            SyntaxToken? semicolonToken = null;
            string? groupId = null;

            if (node is TypeDeclarationSyntax typeDeclarationSyntax)
            {
                typeParameterList = typeDeclarationSyntax.TypeParameterList;
                constraintClauses = typeDeclarationSyntax.ConstraintClauses;
                hasMembers = typeDeclarationSyntax.Members.Count > 0;
                if (typeDeclarationSyntax.Members.Count > 0)
                {
                    members = Docs.Indent(
                        Docs.HardLine,
                        Join(
                            Docs.HardLine,
                            typeDeclarationSyntax.Members.Select(this.Print)
                        )
                    );
                }
                if (node is ClassDeclarationSyntax classDeclarationSyntax)
                {
                    keyword = classDeclarationSyntax.Keyword;
                }
                else if (
                    node is StructDeclarationSyntax structDeclarationSyntax
                ) {
                    keyword = structDeclarationSyntax.Keyword;
                }
                else if (
                    node is InterfaceDeclarationSyntax interfaceDeclarationSyntax
                ) {
                    keyword = interfaceDeclarationSyntax.Keyword;
                }
                else if (
                    node is RecordDeclarationSyntax recordDeclarationSyntax
                ) {
                    keyword = recordDeclarationSyntax.Keyword;
                    groupId = Guid.NewGuid().ToString();
                    parameterList = recordDeclarationSyntax.ParameterList;
                }

                semicolonToken = typeDeclarationSyntax.SemicolonToken;
            }
            else if (node is EnumDeclarationSyntax enumDeclarationSyntax)
            {
                members = Docs.Indent(
                    Docs.HardLine,
                    this.PrintSeparatedSyntaxList(
                        enumDeclarationSyntax.Members,
                        this.PrintEnumMemberDeclarationSyntax,
                        Docs.HardLine
                    )
                );
                hasMembers = enumDeclarationSyntax.Members.Count > 0;
                keyword = enumDeclarationSyntax.EnumKeyword;
                semicolonToken = enumDeclarationSyntax.SemicolonToken;
            }

            var docs = new List<Doc>
            {
                this.PrintExtraNewLines(node),
                this.PrintAttributeLists(node, node.AttributeLists),
            };

            this.PrintModifiers(node.Modifiers, docs);

            if (keyword != null)
            {
                SyntaxTokens.Print(keyword.Value, docs);
                docs.Add(" ");
            }

            SyntaxTokens.Print(node.Identifier, docs);

            if (parameterList != null)
            {
                docs.Add(this.PrintParameterListSyntax(parameterList, groupId));
            }

            if (typeParameterList != null)
            {
                docs.Add(this.PrintTypeParameterListSyntax(typeParameterList));
            }

            if (node.BaseList != null)
            {
                docs.Add(this.PrintBaseListSyntax(node.BaseList));
            }

            docs.Add(this.PrintConstraintClauses(node, constraintClauses));

            if (hasMembers)
            {
                DocUtilities.RemoveInitialDoubleHardLine(members);

                docs.Add(
                    groupId != null
                        ? Docs.IfBreak(" ", Docs.Line, groupId)
                        : Docs.HardLine
                );

                SyntaxTokens.Print(node.OpenBraceToken, docs);
                docs.Add(members, Docs.HardLine);
                SyntaxTokens.Print(node.CloseBraceToken, docs);
            }
            else if (node.OpenBraceToken.Kind() != SyntaxKind.None)
            {
                Doc separator = node.CloseBraceToken.LeadingTrivia.Any()
                    ? Docs.Line
                    : " ";

                docs.Add(
                    separator,
                    SyntaxTokens.Print(node.OpenBraceToken),
                    separator,
                    SyntaxTokens.Print(node.CloseBraceToken)
                );
            }

            if (semicolonToken.HasValue)
            {
                SyntaxTokens.Print(semicolonToken.Value, docs);
            }

            return Docs.Concat(docs);
        }
    }
}
