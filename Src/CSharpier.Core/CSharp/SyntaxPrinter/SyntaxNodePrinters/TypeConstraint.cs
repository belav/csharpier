using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class TypeConstraint
{
    public static Doc Print(TypeConstraintSyntax node, CSharpPrintingContext context)
    {
        return Node.Print(node.Type, context);
    }
}
