using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class NameEquals
    {
        public static Doc Print(NameEqualsSyntax node)
        {
            return Docs.Concat(
                SyntaxNodes.Print(node.Name),
                " ",
                SyntaxTokens.PrintWithSuffix(node.EqualsToken, " ")
            );
        }
    }
}
