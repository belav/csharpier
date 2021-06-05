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
                AttributeLists.Print(node, node.AttributeLists),
                Token.PrintWithSuffix(node.VarianceKeyword, " "),
                Token.Print(node.Identifier)
            );
        }
    }
}
