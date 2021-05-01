using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class WhenClause
    {
        public static Doc Print(WhenClauseSyntax node)
        {
            return Doc.Concat(
                Token.PrintWithSuffix(node.WhenKeyword, " "),
                Node.Print(node.Condition)
            );
        }
    }
}
