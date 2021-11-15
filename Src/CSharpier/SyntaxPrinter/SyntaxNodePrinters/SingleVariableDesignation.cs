using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class SingleVariableDesignation
{
    public static Doc Print(SingleVariableDesignationSyntax node)
    {
        return Token.Print(node.Identifier);
    }
}
