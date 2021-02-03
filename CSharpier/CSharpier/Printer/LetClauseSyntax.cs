using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintLetClauseSyntax(LetClauseSyntax node)
        {
            return Concat(String("let "), String(node.Identifier.Text), String(" = "), this.Print(node.Expression));
        }
    }
}
