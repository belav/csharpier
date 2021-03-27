using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintFinallyClauseSyntax(FinallyClauseSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.FinallyKeyword),
                this.Print(node.Block)
            );
        }
    }
}
