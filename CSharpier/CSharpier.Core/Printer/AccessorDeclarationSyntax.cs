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
                parts.Push(HardLine);
            }
            else
            {
                parts.Push(Line);
            }

            parts.Push(this.PrintAttributeLists(node, node.AttributeLists));
            parts.Push(this.PrintModifiers(node.Modifiers));
            parts.Push(this.PrintSyntaxToken(node.Keyword));

            if (node.Body != null)
            {
                parts.Add(this.PrintBlockSyntax(node.Body));
            }
            else if (node.ExpressionBody != null)
            {
                parts.Add(this.PrintArrowExpressionClauseSyntax(node.ExpressionBody));
            }

            parts.Push(this.PrintSyntaxToken(node.SemicolonToken));

            return Concat(parts);
        }
    }
}