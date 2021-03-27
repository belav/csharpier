using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintWhenClauseSyntax(WhenClauseSyntax node)
        {
            return Concat(
                this.PrintSyntaxToken(node.WhenKeyword, " "),
                this.Print(node.Condition)
            );
        }
    }
}
