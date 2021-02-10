using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintCheckedExpressionSyntax(CheckedExpressionSyntax node)
        {
            return Concat(node.Keyword.Text, "(", this.Print(node.Expression), ")");
        }
    }
}
