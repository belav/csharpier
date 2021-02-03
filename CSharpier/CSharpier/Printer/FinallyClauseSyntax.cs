using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintFinallyClauseSyntax(FinallyClauseSyntax node)
        {
            return Concat(String(node.FinallyKeyword.Text), this.Print(node.Block));
        }
    }
}
