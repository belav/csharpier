using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintImplicitElementAccessSyntax(
            ImplicitElementAccessSyntax node
        ) {
            return this.Print(node.ArgumentList);
        }
    }
}
