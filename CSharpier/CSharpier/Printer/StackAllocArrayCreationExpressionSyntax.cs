using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintStackAllocArrayCreationExpressionSyntax(StackAllocArrayCreationExpressionSyntax node)
        {
            return Concat(
                String("stackalloc "),
                this.Print(node.Type),
                node.Initializer != null
                    ? Concat(String(" "), this.PrintInitializerExpressionSyntax(node.Initializer))
                    : ""
            );
        }
    }
}
