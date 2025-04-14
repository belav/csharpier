using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class SingleVariableDesignation
{
    public static Doc Print(SingleVariableDesignationSyntax node, PrintingContext context)
    {
        return Token.Print(node.Identifier, context);
    }
}
