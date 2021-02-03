using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintElementBindingExpressionSyntax(ElementBindingExpressionSyntax node)
        {
            return this.Print(node.ArgumentList);
        }
    }
}
