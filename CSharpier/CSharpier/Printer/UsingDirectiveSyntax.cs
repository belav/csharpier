using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintUsingDirectiveSyntax(UsingDirectiveSyntax node)
        {
            this.printNewLinesInLeadingTrivia.Push(true);
            var parts = new Parts();
            parts.Push(this.PrintLeadingTrivia(node.UsingKeyword));
            parts.Add(node.UsingKeyword.Text + " ");
            parts.Push(this.PrintTrailingTrivia(node.UsingKeyword));
            if (node.StaticKeyword.RawKind != 0)
            {
                parts.Push(this.PrintLeadingTrivia(node.StaticKeyword));
                parts.Add(node.StaticKeyword.Text + " ");
                parts.Push(this.PrintTrailingTrivia(node.StaticKeyword));
            }
            if (node.Alias != null) {
                parts.Add(this.PrintNameEqualsSyntax(node.Alias));
            }
            parts.Push(this.Print(node.Name), ";");
            parts.Push(this.PrintTrailingTrivia(node.SemicolonToken));
            return Concat(parts);
        }
    }
}
