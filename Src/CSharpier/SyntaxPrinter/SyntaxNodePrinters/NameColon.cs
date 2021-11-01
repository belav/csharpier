using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class NameColon
    {
        public static Doc Print(NameColonSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.Name.Identifier),
                Token.PrintWithSuffix(
                    node.ColonToken,
                    node.Parent
                        is SubpatternSyntax { Pattern: RecursivePatternSyntax { Type: null } }
                      ? Doc.Line
                      : " "
                )
            );
        }
    }
}
