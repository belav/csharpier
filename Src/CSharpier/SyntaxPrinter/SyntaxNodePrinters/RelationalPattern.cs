using System.Linq;
using CSharpier.DocTypes;
using CSharpier.SyntaxPrinter;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class RelationalPattern
    {
        public static Doc Print(RelationalPatternSyntax node)
        {
            return Doc.Concat(
                Token.PrintWithSuffix(node.OperatorToken, " "),
                Node.Print(node.Expression)
            );
        }
    }
}
