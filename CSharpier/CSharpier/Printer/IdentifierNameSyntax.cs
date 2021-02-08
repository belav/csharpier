using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintIdentifierNameSyntax(IdentifierNameSyntax node)
        {
            var parts = new Parts();
            this.PrintLeadingTrivia(node, parts);
            parts.Push(node.Identifier.Text);
            this.PrintTrailingTrivia(node, parts);
            return Concat(parts);
        }
    }
}
