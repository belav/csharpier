using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class InterpolatedStringText
    {
        public static Doc Print(InterpolatedStringTextSyntax node)
        {
            return Token.Print(node.TextToken);
        }
    }
}
