using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintConstructorConstraintSyntax(ConstructorConstraintSyntax node)
        {
            return String("new()");
        }
    }
}
