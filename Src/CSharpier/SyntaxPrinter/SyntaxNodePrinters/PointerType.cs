using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class PointerType
    {
        public static Doc Print(PointerTypeSyntax node)
        {
            return Doc.Concat(Node.Print(node.ElementType), Token.Print(node.AsteriskToken));
        }
    }
}
