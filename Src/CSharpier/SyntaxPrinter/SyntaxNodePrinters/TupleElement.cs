using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class TupleElement
    {
        public static Doc Print(TupleElementSyntax node)
        {
            return Doc.Concat(
                Node.Print(node.Type),
                node.Identifier.Kind() != SyntaxKind.None
                  ? Doc.Concat(" ", Token.Print(node.Identifier))
                  : Doc.Null
            );
        }
    }
}
