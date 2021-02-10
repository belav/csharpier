using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintVarPatternSyntax(VarPatternSyntax node)
        {
            return Concat("var ", this.Print(node.Designation));
        }
    }
}
