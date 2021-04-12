using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintElementAccessExpressionSyntax(
            ElementAccessExpressionSyntax node
        ) {
            return Docs.Concat(
                this.Print(node.Expression),
                this.Print(node.ArgumentList)
            );
        }
    }
}
