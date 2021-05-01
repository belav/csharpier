using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class PointerType
    {
        public static Doc Print(PointerTypeSyntax node)
        {
            return Doc.Concat(
                Node.Print(node.ElementType),
                Token.Print(node.AsteriskToken)
            );
        }
    }
}
