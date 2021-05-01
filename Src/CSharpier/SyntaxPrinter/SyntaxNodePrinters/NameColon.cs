using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class NameColon
    {
        public static Doc Print(NameColonSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.Name.Identifier),
                Token.Print(node.ColonToken, " ")
            );
        }
    }
}
