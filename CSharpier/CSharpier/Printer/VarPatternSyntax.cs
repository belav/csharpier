using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintVarPatternSyntax(VarPatternSyntax node)
        {
            return Concat(String("var "), this.Print(node.Designation));
        }
    }
}
