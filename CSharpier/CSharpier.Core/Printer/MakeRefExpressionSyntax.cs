using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintMakeRefExpressionSyntax(MakeRefExpressionSyntax node)
        {
            return Concat(
                node.Keyword.Text,
                "(",
                this.Print(node.Expression),
                ")"
            );
        }
    }
}