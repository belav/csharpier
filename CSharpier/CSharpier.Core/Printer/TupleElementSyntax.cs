using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintTupleElementSyntax(TupleElementSyntax node)
        {
            return this.Print(node.Type);
        }
    }
}
