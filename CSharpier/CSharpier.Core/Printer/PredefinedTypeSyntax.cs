using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintPredefinedTypeSyntax(PredefinedTypeSyntax node)
        {
            return Concat(this.PrintLeadingTrivia(node), node.Keyword.Text, this.PrintTrailingTrivia(node));
        }
    }
}
