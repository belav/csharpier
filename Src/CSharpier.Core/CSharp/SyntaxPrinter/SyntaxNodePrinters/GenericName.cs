using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class GenericName
{
    public static Doc Print(GenericNameSyntax node, CSharpPrintingContext context)
    {
        return Doc.Group(
            Token.Print(node.Identifier, context),
            Node.Print(node.TypeArgumentList, context)
        );
    }
}
