using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintExternAliasDirectiveSyntax(ExternAliasDirectiveSyntax node)
        {
            var parts = new Parts();
            this.PrintLeadingTrivia(node.ExternKeyword.LeadingTrivia, parts);
            parts.Push(node.ExternKeyword.Text, " ", node.AliasKeyword.Text, " ", node.Identifier.Text, ";");
            return Concat(parts);
        }
    }
}
