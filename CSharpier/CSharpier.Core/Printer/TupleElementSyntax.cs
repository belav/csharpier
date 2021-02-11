using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintTupleElementSyntax(TupleElementSyntax node)
        {
            return Concat(this.Print(node.Type),
                    node.Identifier.RawKind != 0 ? Concat(" ", this.PrintSyntaxToken(node.Identifier)) : null);
        }
    }
}
