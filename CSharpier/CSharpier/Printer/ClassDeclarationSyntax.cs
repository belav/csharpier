using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private void PrintExtraNewLines(ClassDeclarationSyntax node, Parts parts)
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

            foreach (var trivia in node.Keyword.LeadingTrivia)
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
        
        // TODO ClassDeclarationSyntax is TypeDeclarationSyntax, but enum is not? interface, record and struct are all TypeDeclarationSyntax
        private Doc PrintClassDeclarationSyntax(ClassDeclarationSyntax node)
        {
            var parts = new Parts();
            
            this.PrintExtraNewLines(node, parts);

            this.PrintAttributeLists(node, node.AttributeLists, parts);
            // TODO printLeadingComments(node, parts, String("modifiers"), String("keyword"), String("identifier"));
            parts.Add(this.PrintModifiers(node.Modifiers));
            if (node.Keyword.RawKind != 0)
            {
                parts.Add(node.Keyword.Text);
            }

            // if (node.EnumKeyword != null) {
            //     parts.Add(printSyntaxToken(node.EnumKeyword));
            // }
            parts.Push(String(" "), node.Identifier.Text);
            if (node.TypeParameterList != null)
            {
                parts.Add(this.PrintTypeParameterListSyntax(node.TypeParameterList));
            }

            if (node.BaseList != null)
            {
                parts.Add(this.PrintBaseListSyntax(node.BaseList));
            }

            this.PrintConstraintClauses(node, node.ConstraintClauses, parts);
            var hasMembers = node.Members.Count > 0;
            if (hasMembers)
            {
                parts.Add(Concat(node.ConstraintClauses.Count > 0 ? "" : HardLine, "{"));
                var LineSeparator = HardLine;
                if (node is EnumDeclarationSyntax)
                {
                    LineSeparator = Concat(String(","), HardLine);
                }

                parts.Add(Indent(Concat(HardLine, Join(LineSeparator, node.Members.Select(this.Print)))));
                parts.Add(HardLine);
                parts.Add(String("}"));
            }
            else
            {
                parts.Push(node.ConstraintClauses.Count > 0 ? "" : " ", "{", " ", "}");
            }

            return Concat(parts);
        }
    }
}