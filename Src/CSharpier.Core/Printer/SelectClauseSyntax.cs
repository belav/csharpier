using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintSelectClauseSyntax(SelectClauseSyntax node)
        {
            return Concat(this.PrintSyntaxToken(node.SelectKeyword, " "), this.Print(node.Expression));
        }
    }
}
