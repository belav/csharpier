using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypePatternSyntax(TypePatternSyntax node)
        {
            return this.Print(node.Type);
        }
    }
}
