using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class TypePattern
    {
        public static Doc Print(TypePatternSyntax node)
        {
            return Node.Print(node.Type);
        }
    }
}
