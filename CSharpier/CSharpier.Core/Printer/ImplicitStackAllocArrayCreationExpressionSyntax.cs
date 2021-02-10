using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintImplicitStackAllocArrayCreationExpressionSyntax(ImplicitStackAllocArrayCreationExpressionSyntax node)
        {
            return Concat(node.StackAllocKeyword.Text, "[] ", this.Print(node.Initializer));
        }
    }
}
