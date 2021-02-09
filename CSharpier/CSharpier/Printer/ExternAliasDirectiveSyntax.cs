using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintExternAliasDirectiveSyntax(ExternAliasDirectiveSyntax node)
        {
            this.printNewLinesInLeadingTrivia.Push(true);
            var parts = new Parts();
            parts.Push(this.PrintLeadingTrivia(node.ExternKeyword));
            this.printNewLinesInLeadingTrivia.Pop();
            parts.Push(node.ExternKeyword.Text, " ");
            parts.Push(this.PrintTrailingTrivia(node.ExternKeyword));
            parts.Push(this.PrintLeadingTrivia(node.AliasKeyword));
            parts.Push(node.AliasKeyword.Text, " ");
            parts.Push(this.PrintTrailingTrivia(node.AliasKeyword));
            parts.Push(this.PrintLeadingTrivia(node.Identifier));
            parts.Push(node.Identifier.Text, ";");
            parts.Push(this.PrintTrailingTrivia(node.SemicolonToken));
            return Concat(parts);
        }
    }
}
