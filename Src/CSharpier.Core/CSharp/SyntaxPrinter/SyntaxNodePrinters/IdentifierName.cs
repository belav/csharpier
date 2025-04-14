using CSharpier.Core.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core.CSharp.SyntaxPrinter.SyntaxNodePrinters;

internal static class IdentifierName
{
    public static Doc Print(IdentifierNameSyntax node, PrintingContext context)
    {
        return Token.Print(node.Identifier, context);
    }
}
