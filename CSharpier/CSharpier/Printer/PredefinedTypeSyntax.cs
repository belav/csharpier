using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintPredefinedTypeSyntax(PredefinedTypeSyntax node)
        {
            var parts = new Parts();
            this.PrintLeadingTrivia(node, parts);
            parts.Push(node.Keyword.Text);
            this.PrintTrailingTrivia(node, parts);
            return Concat(parts);
        }
    }
}
