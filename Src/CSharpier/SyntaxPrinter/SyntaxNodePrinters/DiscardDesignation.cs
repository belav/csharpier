using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class DiscardDesignation
    {
        public static Doc Print(DiscardDesignationSyntax node)
        {
            return Token.Print(node.UnderscoreToken);
        }
    }
}
