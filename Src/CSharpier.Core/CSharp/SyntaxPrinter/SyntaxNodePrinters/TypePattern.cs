using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypePattern
{
    public static Doc Print(TypePatternSyntax node, PrintingContext context)
    {
        return Node.Print(node.Type, context);
    }
}
