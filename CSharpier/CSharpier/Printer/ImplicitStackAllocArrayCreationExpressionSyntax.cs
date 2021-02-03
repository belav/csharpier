using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintImplicitStackAllocArrayCreationExpressionSyntax(ImplicitStackAllocArrayCreationExpressionSyntax node)
        {
            return Concat(String(node.StackAllocKeyword.Text), String("[] "), this.Print(node.Initializer));
        }
    }
}
