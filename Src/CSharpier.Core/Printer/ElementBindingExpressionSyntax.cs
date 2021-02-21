using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintElementBindingExpressionSyntax(ElementBindingExpressionSyntax node)
        {
            return this.Print(node.ArgumentList);
        }
    }
}
