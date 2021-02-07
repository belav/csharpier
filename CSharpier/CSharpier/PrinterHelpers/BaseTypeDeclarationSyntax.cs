using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintBaseTypeDeclarationSyntax(BaseTypeDeclarationSyntax node)
        {
            TypeParameterListSyntax typeParameterList = null;
            var hasConstraintClauses = false;
            var constraintClauses = Enumerable.Empty<TypeParameterConstraintClauseSyntax>();
            var hasMembers = false;
            SyntaxToken? keyword = null;
            var memberSeparator = HardLine;
            var members = Enumerable.Empty<CSharpSyntaxNode>();

            if (node is TypeDeclarationSyntax typeDeclarationSyntax)
            {
                typeParameterList = typeDeclarationSyntax.TypeParameterList;
                constraintClauses = typeDeclarationSyntax.ConstraintClauses;
                hasConstraintClauses = typeDeclarationSyntax.ConstraintClauses.Count > 0;
                members = typeDeclarationSyntax.Members;
                hasMembers = typeDeclarationSyntax.Members.Count > 0;
                if (node is ClassDeclarationSyntax classDeclarationSyntax)
                {
                    keyword = classDeclarationSyntax.Keyword;
                }
                else if (node is StructDeclarationSyntax structDeclarationSyntax)
                {
                    keyword = structDeclarationSyntax.Keyword;
                }
                else if (node is InterfaceDeclarationSyntax interfaceDeclarationSyntax)
                {
                    keyword = interfaceDeclarationSyntax.Keyword;
                }
            }
            else if (node is EnumDeclarationSyntax enumDeclarationSyntax)
            {
                members = enumDeclarationSyntax.Members;
                hasMembers = enumDeclarationSyntax.Members.Count > 0;
                keyword = enumDeclarationSyntax.EnumKeyword;
                memberSeparator = Concat(String(","), HardLine);
            }

            var parts = new Parts();

            this.PrintAttributeLists(node, node.AttributeLists, parts);
            var printedExtraNewLines = false;
            parts.Add(this.PrintModifiers(node.Modifiers, ref printedExtraNewLines));
            if (keyword != null)
            {
                PrintLeadingTrivia(keyword.Value.LeadingTrivia, parts, ref printedExtraNewLines, true);
                parts.Add(keyword.Value.Text);
                if (!PrintTrailingTrivia(keyword.Value.TrailingTrivia, parts))
                {
                    parts.Push(" ");
                }
            }

            parts.Push(node.Identifier.Text);
            if (typeParameterList != null)
            {
                parts.Add(this.PrintTypeParameterListSyntax(typeParameterList));
            }

            if (node.BaseList != null)
            {
                parts.Add(this.PrintBaseListSyntax(node.BaseList));
            }

            this.PrintConstraintClauses(node, constraintClauses, parts);

            if (hasMembers)
            {
                parts.Add(Concat(hasConstraintClauses ? "" : HardLine, "{"));
                parts.Add(Indent(Concat(HardLine, Join(memberSeparator, members.Select(this.Print)))));
                parts.Add(HardLine);
                parts.Add(String("}"));
            }
            else
            {
                parts.Push(hasConstraintClauses ? "" : " ", "{", " ", "}");
            }

            return Concat(parts);
        }
    }
}