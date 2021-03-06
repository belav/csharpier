using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class ConstantPattern
    {
        public static Doc Print(ConstantPatternSyntax node)
        {
            return Node.Print(node.Expression);
        }
    }
}
