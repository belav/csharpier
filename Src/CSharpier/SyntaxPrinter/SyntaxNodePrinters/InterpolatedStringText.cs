using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters;

internal static class InterpolatedStringText
{
    public static Doc Print(InterpolatedStringTextSyntax node)
    {
        return Token.Print(node.TextToken);
    }
}
