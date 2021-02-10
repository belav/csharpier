using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintFinallyClauseSyntax(FinallyClauseSyntax node)
        {
            return Concat(node.FinallyKeyword.Text, this.Print(node.Block));
        }
    }
}
