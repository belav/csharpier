using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTypeConstraintSyntax(TypeConstraintSyntax node)
        {
            return this.Print(node.Type);
        }
    }
}
