using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
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
                memberSeparator = Concat(",", HardLine);
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

            if (typeParameterList != null)
            {
                parts.Push(this.PrintTypeParameterListSyntax(typeParameterList));
            }

            if (node.BaseList != null)
            {
                parts.Push(this.PrintBaseListSyntax(node.BaseList));
            }

            this.PrintConstraintClauses(node, constraintClauses, parts);

            if (hasMembers)
            {
                parts.Push(Concat(hasConstraintClauses ? "" : HardLine, "{"));
                parts.Push(Indent(Concat(HardLine, Join(memberSeparator, members.Select(this.Print)))));
                parts.Push(HardLine);
                parts.Push("}");
            }
            else
            {
                parts.Push(hasConstraintClauses ? "" : " ", "{", " ", "}");
            }

            return Concat(parts);
        }
    }
}