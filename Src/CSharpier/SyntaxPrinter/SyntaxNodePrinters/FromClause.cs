using CSharpier.DocTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.SyntaxPrinter.SyntaxNodePrinters
{
    public static class FromClause
    {
        public static Doc Print(FromClauseSyntax node)
        {
            return Doc.Concat(
                Token.Print(node.FromKeyword, " "),
                node.Type != null ? Doc.Concat(Node.Print(node.Type), " ") : Doc.Null,
                Token.Print(node.Identifier, " "),
                Token.Print(node.InKeyword, " "),
                Node.Print(node.Expression)
            );
        }
    }
}
