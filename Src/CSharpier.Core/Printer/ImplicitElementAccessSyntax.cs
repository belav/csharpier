using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintImplicitElementAccessSyntax(ImplicitElementAccessSyntax node)
        {
            return this.Print(node.ArgumentList);
        }
    }
}
