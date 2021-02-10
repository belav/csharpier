using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintSizeOfExpressionSyntax(SizeOfExpressionSyntax node)
        {
            return Concat(node.Keyword.Text, "(", this.Print(node.Type), ")");
        }
    }
}
