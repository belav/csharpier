using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBaseTypeDeclarationSyntax(
            BaseTypeDeclarationSyntax node)
        {
            ParameterListSyntax? parameterList = null;
            TypeParameterListSyntax? typeParameterList = null;
            var constraintClauses = Enumerable.Empty<TypeParameterConstraintClauseSyntax>();
            var hasMembers = false;
            SyntaxToken? keyword = null;
            Doc members = Doc.Null;
            SyntaxToken? semicolonToken = null;

            if (node is TypeDeclarationSyntax typeDeclarationSyntax)
            {
                typeParameterList = typeDeclarationSyntax.TypeParameterList;
                constraintClauses = typeDeclarationSyntax.ConstraintClauses;
                hasMembers = typeDeclarationSyntax.Members.Count > 0;
                if (typeDeclarationSyntax.Members.Count > 0)
                {
                    members = Indent(
                        HardLine,
                        Join(
                            HardLine,
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
                )
                {
                    keyword = structDeclarationSyntax.Keyword;
                }
                else if (
                    node is InterfaceDeclarationSyntax interfaceDeclarationSyntax
                )
                {
                    keyword = interfaceDeclarationSyntax.Keyword;
                }
                else if (
                    node is RecordDeclarationSyntax recordDeclarationSyntax
                )
                {
                    keyword = recordDeclarationSyntax.Keyword;
                    parameterList = recordDeclarationSyntax.ParameterList;
                }

                semicolonToken = typeDeclarationSyntax.SemicolonToken;
            }
            else if (node is EnumDeclarationSyntax enumDeclarationSyntax)
            {
                members = Indent(
                    HardLine,
                    this.PrintSeparatedSyntaxList(
                        enumDeclarationSyntax.Members,
                        this.PrintEnumMemberDeclarationSyntax,
                        HardLine
                    )
                );
                hasMembers = enumDeclarationSyntax.Members.Count > 0;
                keyword = enumDeclarationSyntax.EnumKeyword;
                semicolonToken = enumDeclarationSyntax.SemicolonToken;
            }

            var parts = new Parts();
            parts.Push(this.PrintExtraNewLines(node));
            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            parts.Push(this.PrintModifiers(node.Modifiers));
            if (keyword != null)
            {
                parts.Push(this.PrintSyntaxToken(keyword.Value, " "));
            }

            parts.Push(this.PrintSyntaxToken(node.Identifier));

            if (parameterList != null)
            {
                parts.Push(this.PrintParameterListSyntax(parameterList));
            }

            if (typeParameterList != null)
            {
                parts.Push(
                    this.PrintTypeParameterListSyntax(typeParameterList)
                );
            }

            if (node.BaseList != null)
            {
                parts.Push(this.PrintBaseListSyntax(node.BaseList));
            }

            parts.Push(this.PrintConstraintClauses(node, constraintClauses));

            if (hasMembers)
            {
                parts.Push(
                    HardLine,
                    this.PrintSyntaxToken(node.OpenBraceToken),
                    members,
                    HardLine,
                    this.PrintSyntaxToken(node.CloseBraceToken)
                );
            }
            else if (node.OpenBraceToken.Kind() != SyntaxKind.None)
            {
                parts.Push(
                    " ",
                    this.PrintSyntaxToken(node.OpenBraceToken),
                    " ",
                    this.PrintSyntaxToken(node.CloseBraceToken)
                );
            }

            if (semicolonToken.HasValue)
            {
                parts.Push(this.PrintSyntaxToken(semicolonToken.Value));
            }

            return Concat(parts);
        }
    }
}
