using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class BaseExpressionColon
    {
        public static Doc Print(BaseExpressionColonSyntax node)
        {
            return Doc.Concat(
                Node.Print(node.Expression),
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
