using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class TupleElement
{
    public static Doc Print(TupleElementSyntax node, PrintingContext context)
    {
        return Doc.Concat(
            Node.Print(node.Type, context),
            node.Identifier.RawSyntaxKind() != SyntaxKind.None
                ? Doc.Concat(" ", Token.Print(node.Identifier, context))
                : Doc.Null
        );
    }
}
