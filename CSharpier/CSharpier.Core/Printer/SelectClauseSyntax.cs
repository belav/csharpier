using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintSelectClauseSyntax(SelectClauseSyntax node)
        {
            return Concat(node.SelectKeyword.Text, " ", this.Print(node.Expression));
        }
    }
}
