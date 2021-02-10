using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintConstructorConstraintSyntax(ConstructorConstraintSyntax node)
        {
            return "new()";
        }
    }
}
