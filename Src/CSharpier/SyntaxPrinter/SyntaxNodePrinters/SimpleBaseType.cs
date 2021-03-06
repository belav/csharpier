using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class SimpleBaseType
    {
        public static Doc Print(SimpleBaseTypeSyntax node)
        {
            return Node.Print(node.Type);
        }
    }
}
