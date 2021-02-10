using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintConstantPatternSyntax(ConstantPatternSyntax node)
        {
            return this.Print(node.Expression);
        }
    }
}
