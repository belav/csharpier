using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintAccessorDeclarationSyntax(AccessorDeclarationSyntax node)
        {
            var parts = new Parts();
            if (node.Modifiers.Count > 0 || node.AttributeLists.Count > 0 || NotNull(node.Body) || NotNull(node.ExpressionBody))
            {
                parts.Push(HardLine);
            }
            else
            {
                parts.Push(Line);
            }

            this.PrintAttributeLists(node.AttributeLists, parts);
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(node.Keyword.Text);
            if (!NotNull(node.Body) && !NotNull(node.ExpressionBody))
            {
                parts.Push(String(";"));
            }
            else
            {
                if (NotNull(node.Body))
                {
                    parts.Push(this.PrintBlockSyntax(node.Body));
                }
                else
                {
                    parts.Push(this.PrintArrowExpressionClauseSyntax(node.ExpressionBody));
                    parts.Push(String(";"));
                }
            }

            return Concat(parts);
        }
    }
}