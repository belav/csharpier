using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class PredefinedType
    {
        public static Doc Print(PredefinedTypeSyntax node)
        {
            return Token.Print(node.Keyword);
        }
    }
}
