using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class NameEquals
    {
        public static Doc Print(NameEqualsSyntax node)
        {
            return Doc.Concat(
                Node.Print(node.Name),
                " ",
                Token.PrintWithSuffix(node.EqualsToken, " ")
            );
        }
    }
}
