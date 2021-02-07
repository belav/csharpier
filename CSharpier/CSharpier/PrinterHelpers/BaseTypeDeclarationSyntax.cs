using System.Linq;
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
            string keyword = null;
            var memberSeparator = HardLine;
            var members = Enumerable.Empty<CSharpSyntaxNode>();

            if (node is TypeDeclarationSyntax typeDeclarationSyntax)
            {
                typeParameterList = typeDeclarationSyntax.TypeParameterList;
                constraintClauses = typeDeclarationSyntax.ConstraintClauses;
                hasConstraintClauses = typeDeclarationSyntax.ConstraintClauses.Count > 0;
                members = typeDeclarationSyntax.Members;
                hasMembers = typeDeclarationSyntax.Members.Count > 0;
                if (node is ClassDeclarationSyntax)
                {
                    keyword = "class";
                }
                else if (node is StructDeclarationSyntax)
                {
                    keyword = "struct";
                }
                else if (node is InterfaceDeclarationSyntax)
                {
                    keyword = "interface";
                }
            }
            else if (node is EnumDeclarationSyntax enumDeclarationSyntax)
            {
                members = enumDeclarationSyntax.Members;
                hasMembers = enumDeclarationSyntax.Members.Count > 0;
                keyword = "enum";
                memberSeparator = Concat(String(","), HardLine);
            }

            var parts = new Parts();

            this.PrintExtraNewLines(node, parts);

            this.PrintAttributeLists(node, node.AttributeLists, parts);
            // TODO printLeadingComments(node, parts, String("modifiers"), String("keyword"), String("identifier"));
            parts.Add(this.PrintModifiers(node.Modifiers));
            if (keyword != null)
            {
                parts.Add(keyword);
            }

            parts.Push(String(" "), node.Identifier.Text);
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


        // TODO 0 how do I really do extra new lines?
        private void PrintExtraNewLines(BaseTypeDeclarationSyntax node, Parts parts)
        {
            // TODO attribute lists?
            if (node.Modifiers.Count > 0)
            {
                foreach (var trivia in node.Modifiers[0].LeadingTrivia)
                {
                    if (trivia.Kind() == SyntaxKind.EndOfLineTrivia)
                    {
                        parts.Push(HardLine);
                    }
                    else if (trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
                    {
                        return;
                    }
                }
            }

            if (node is ClassDeclarationSyntax classDeclarationSyntax)
            {
                foreach (var trivia in classDeclarationSyntax.Keyword.LeadingTrivia)
                {
                    if (trivia.Kind() == SyntaxKind.EndOfLineTrivia)
                    {
                        parts.Push(HardLine);
                    }
                    else if (trivia.Kind() == SyntaxKind.SingleLineCommentTrivia)
                    {
                        return;
                    }
                }
            }
        }

    }
}