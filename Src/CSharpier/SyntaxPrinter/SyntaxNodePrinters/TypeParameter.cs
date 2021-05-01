using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class TypeParameter
    {
        public static Doc Print(TypeParameterSyntax node)
        {
            return Doc.Concat(
                new Printer().PrintAttributeLists(node, node.AttributeLists),
                Token.Print(node.VarianceKeyword, " "),
                Token.Print(node.Identifier)
            );
        }
    }
}
