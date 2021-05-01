using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintTupleElementSyntax(TupleElementSyntax node)
        {
            return Doc.Concat(
                this.Print(node.Type),
                node.Identifier.RawKind != 0
                    ? Doc.Concat(" ", Token.Print(node.Identifier))
                    : Doc.Null
            );
        }
    }
}
