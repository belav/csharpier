using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintWhenClauseSyntax(WhenClauseSyntax node)
        {
            return Concat(node.WhenKeyword.Text, " ", this.Print(node.Condition));
        }
    }
}
