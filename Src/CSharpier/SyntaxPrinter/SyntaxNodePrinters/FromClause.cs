using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class FromClause
    {
        public static Doc Print(FromClauseSyntax node)
        {
            return Doc.Concat(
                Token.PrintWithSuffix(node.FromKeyword, " "),
                node.Type != null ? Doc.Concat(Node.Print(node.Type), " ") : Doc.Null,
                Token.PrintWithSuffix(node.Identifier, " "),
                Token.PrintWithSuffix(node.InKeyword, " "),
                Node.Print(node.Expression)
            );
        }
    }
}
