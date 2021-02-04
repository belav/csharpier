using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        // TODO ClassDeclarationSyntax is TypeDeclarationSyntax, but enum is not? interface, record and struct are all TypeDeclarationSyntax
        private Doc PrintClassDeclarationSyntax(ClassDeclarationSyntax node)
        {
            var parts = new Parts();
            // TODO this.PrintExtraNewLines(node, String("attributeLists"), String("modifiers"), String("keyword"));
            this.PrintAttributeLists(node.AttributeLists, parts);
            // TODO printLeadingComments(node, parts, String("modifiers"), String("keyword"), String("identifier"));
            parts.Push(this.PrintModifiers(node.Modifiers));
            if (node.Keyword.RawKind != 0)
            {
                parts.Push(node.Keyword.Text);
            }

            // if (NotNull(node.EnumKeyword)) {
            //     parts.Push(printSyntaxToken(node.EnumKeyword));
            // }
            parts.Push(String(" "), node.Identifier.Text);
            if (node.TypeParameterList != null)
            {
                parts.Push(this.PrintTypeParameterListSyntax(node.TypeParameterList));
            }

            if (node.BaseList != null)
            {
                parts.Push(this.PrintBaseListSyntax(node.BaseList));
            }

            this.PrintConstraintClauses(node.ConstraintClauses, parts);
            var hasMembers = node.Members.Count > 0;
            if (hasMembers)
            {
                parts.Push(Concat(node.ConstraintClauses.Count > 0 ? "String(" : HardLine, "){"));
                var LineSeparator = HardLine;
                if (node is EnumDeclarationSyntax)
                {
                    LineSeparator = Concat(String(","), HardLine);
                }

                parts.Push(Indent(Concat(HardLine, Join(LineSeparator, node.Members.Select(this.Print)))));
                parts.Push(HardLine);
                parts.Push(String("}"));
            }
            else
            {
                parts.Push(node.ConstraintClauses.Count > 0 ? "" : " ", "{", " ", "}");
            }

            return Concat(parts);
        }
    }
}