using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintIdentifierNameSyntax(IdentifierNameSyntax node)
        {
            return Concat(this.PrintLeadingTrivia(node),
                node.Identifier.Text,
                this.PrintTrailingTrivia(node));
        }
    }
}