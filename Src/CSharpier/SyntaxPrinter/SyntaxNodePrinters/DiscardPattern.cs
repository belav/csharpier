using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class DiscardPattern
    {
        public static Doc Print(DiscardPatternSyntax node)
        {
            return Token.Print(node.UnderscoreToken);
        }
    }
}
