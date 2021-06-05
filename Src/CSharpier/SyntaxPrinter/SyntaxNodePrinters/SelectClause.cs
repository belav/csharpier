using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class SelectClause
    {
        public static Doc Print(SelectClauseSyntax node)
        {
            return Doc.Concat(
                Token.PrintWithSuffix(node.SelectKeyword, " "),
                Node.Print(node.Expression)
            );
        }
    }
}
