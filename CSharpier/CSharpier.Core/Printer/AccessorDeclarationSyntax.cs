using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintAccessorDeclarationSyntax(AccessorDeclarationSyntax node)
        {
            var parts = new Parts();
            if (node.Modifiers.Count > 0 || node.AttributeLists.Count > 0 || node.Body != null || node.ExpressionBody != null)
            {
                parts.Add(HardLine);
            }
            else
            {
                parts.Add(Line);
            }

            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            parts.Add(this.PrintModifiers(node.Modifiers));
            parts.Add(node.Keyword.Text);
            if (node.Body == null && node.ExpressionBody == null)
            {
                parts.Add(";");
            }
            else
            {
                if (node.Body != null)
                {
                    parts.Add(this.PrintBlockSyntax(node.Body));
                }
                else
                {
                    parts.Add(this.PrintArrowExpressionClauseSyntax(node.ExpressionBody));
                    parts.Add(";");
                }
            }

            return Concat(parts);
        }
    }
}