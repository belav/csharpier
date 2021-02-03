using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintSelectClauseSyntax(SelectClauseSyntax node)
        {
            return Concat(String(node.SelectKeyword.Text), String(" "), this.Print(node.Expression));
        }
    }
}
