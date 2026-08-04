using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class IdentifierName
{
    public static Doc Print(IdentifierNameSyntax node, CSharpPrintingContext context)
    {
        return Token.Print(node.Identifier, context);
    }
}
