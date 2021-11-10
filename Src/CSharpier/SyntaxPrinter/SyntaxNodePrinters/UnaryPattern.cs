using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    internal static class UnaryPattern
    {
        public static Doc Print(UnaryPatternSyntax node)
        {
            return Doc.Concat(
                Token.PrintWithSuffix(node.OperatorToken, " "),
                Node.Print(node.Pattern)
            );
        }
    }
}
