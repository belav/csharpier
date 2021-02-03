using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWhenClauseSyntax(WhenClauseSyntax node)
        {
            return Concat(String(node.WhenKeyword.Text), String(" "), this.Print(node.Condition));
        }
    }
}
